using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Data.Provider;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.Data;

public class ClaimRepository : IClaimRepository
{
  private readonly IDataProvider _provider;
  private readonly IAccessValidator _accessValidator;

  private async Task<IQueryable<DbClaim>> CreateFindPredicates(
    FindClaimFilter filter,
    Guid senderId)
  {
    IQueryable<DbClaim> dbClaims = await _accessValidator.IsAdminAsync(senderId)
      ? _provider.Claims.AsNoTracking()
      : _provider.Claims.AsNoTracking().Where(c => c.CreatedBy == senderId);

    if (!string.IsNullOrWhiteSpace(filter.searchSubString))
    {
      dbClaims = dbClaims.Where(c =>
      c.Content.Contains(filter.searchSubString)
      || c.Name.Contains(filter.searchSubString));
    }

    if (filter.CategoryId.HasValue)
    {
      dbClaims = dbClaims.Where(c => c.CategoryId == filter.CategoryId.Value);
    }

    if (filter.Priority.HasValue)
    {
      dbClaims = dbClaims.Where(c => c.Priority == filter.Priority.Value);
    }

    if (filter.Status.HasValue)
    {
      dbClaims = dbClaims.Where(c => c.Status == filter.Status.Value);
    }

    if (filter.DeadLine.HasValue)
    {
      dbClaims = dbClaims.Where(c => c.DeadLine == filter.DeadLine.Value);
    }

    if (filter.AuthorId.HasValue)
    {
      dbClaims = dbClaims.Where(c => c.CreatedBy == filter.AuthorId.Value);
    }

    if (filter.isAscendingSort.HasValue)
    {
      dbClaims = filter.isAscendingSort.Value
        ? dbClaims.OrderBy(c => c.Id)
        : dbClaims.OrderByDescending(c => c.Id);
    }

    return dbClaims;
  }

  private async Task<IQueryable<DbClaim>> CreateGetPredicate(
    GetClaimFilter filter,
    Guid senderId)
  {
    IQueryable<DbClaim> dbClaims = await _accessValidator.IsAdminAsync(senderId)
      ? _provider.Claims.AsNoTracking()
      : _provider.Claims.AsNoTracking().Where(c => c.CreatedBy == senderId);

    return dbClaims;
  }

  public ClaimRepository(
    IDataProvider provider,
    IAccessValidator accessValidator)
  {
    _provider = provider;
    _accessValidator = accessValidator;
  }

  public async Task<Guid?> CreateAsync(DbClaim claim)
  {
    if (claim == null)
    {
      return null;
    }

    _provider.Claims.Add(claim);
    await _provider.SaveAsync();

    return claim.Id;
  }

  public async Task<(List<DbClaim> dbClaims, int totalcount)> FindAsync(
    FindClaimFilter filter,
    Guid senderId,
    CancellationToken cancellationToken = default)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbClaim> dbClaims = await CreateFindPredicates(filter, senderId);

    return (
      await dbClaims
      .Skip(filter.SkipCount)
      .Take(filter.TakeCount)
      .ToListAsync(cancellationToken),
      await dbClaims.CountAsync(cancellationToken));
  }

  public async Task<DbClaim> GetAsync(
    GetClaimFilter filter,
    Guid senderId,
    CancellationToken cancellationToken = default)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbClaim> dbClaims = await CreateGetPredicate(filter, senderId);

    return await dbClaims.AsNoTracking().FirstOrDefaultAsync(c => c.Id == filter.Id, cancellationToken);
  }

  public async Task<DbClaim> EditAsync(
    Guid claimId,
    JsonPatchDocument<DbClaim> patch,
    Guid modifierId,
    CancellationToken cancellationToken = default)
  {
    DbClaim dbClaim = await _provider.Claims.FirstOrDefaultAsync(c => c.Id == claimId, cancellationToken);

    if (dbClaim is null || dbClaim.Status == ClaimStatus.Closed)
    {
      return default;
    }

    patch.ApplyTo(dbClaim);
    dbClaim.ModifiedBy = modifierId;
    dbClaim.ModifiedAtUtc = DateTime.UtcNow;
    await _provider.SaveAsync();

    return dbClaim;
  }
}
