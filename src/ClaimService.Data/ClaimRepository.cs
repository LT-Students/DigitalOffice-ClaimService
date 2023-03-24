using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClaimService.Data.Provider;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.Data;

public class ClaimRepository : IClaimRepository
{
  private readonly IDataProvider _provider;

  private IQueryable<DbClaim> CreateFindPredicates(
    FindClaimFilter filter,
    IQueryable<DbClaim> dbClaims)
  {
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

    if (filter.Urgency.HasValue)
    {
      dbClaims = dbClaims.Where(c => c.Urgency == filter.Urgency.Value);
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

  private IQueryable<DbClaim> CreateGetPredicate(
    GetClaimFilter filter,
    IQueryable<DbClaim> dbClaims)
  {
    return dbClaims;
  }

  public ClaimRepository(IDataProvider provider)
  {
    _provider = provider;
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

  public async Task<(List<DbClaim> dbClaim, int totalcount)> FindAsync(
    FindClaimFilter filter,
    CancellationToken cancellationToken = default)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbClaim> dbClaims = CreateFindPredicates(filter, _provider.Claims.AsNoTracking());

    return (
      await dbClaims
      .Skip(filter.SkipCount)
      .Take(filter.TakeCount)
      .ToListAsync(cancellationToken),
      await dbClaims.CountAsync(cancellationToken));
  }

  public async Task<DbClaim> GetAsync(
    GetClaimFilter filter,
    CancellationToken cancellationToken = default)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbClaim> dbClaims = CreateGetPredicate(filter, _provider.Claims.AsNoTracking());

    return await dbClaims.FirstOrDefaultAsync(c => c.Id == filter.Id, cancellationToken);
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
