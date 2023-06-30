using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaims;

public class GetClaimsHandler : IRequestHandler<GetClaimsQuery, FindResult<ClaimInfo>>
{
  private readonly IDataProvider _provider;
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public GetClaimsHandler(
    IDataProvider provider,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<FindResult<ClaimInfo>> Handle(GetClaimsQuery query, CancellationToken ct)
  {
    (List<DbClaim> claims, int totalCount) = await GetAsync(query, ct);
    return new FindResult<ClaimInfo>
    {
      Body = claims.ConvertAll(Map),
      TotalCount = totalCount
    };
  }

  private ClaimInfo Map(DbClaim claim)
  {
    return new()
    {
      Id = claim.Id,
      Name = claim.Name,
      Content = claim.Content,
      Status = (ClaimStatus)claim.Status,
      Priority = (ClaimPriority)claim.Priority,
      DeadLine = claim.DeadLine,
      CreatedBy = claim.CreatedBy,
      CreatedAtUtc = claim.CreatedAtUtc,
    };
  }

  private async Task<(List<DbClaim> dbClaims, int totalcount)> GetAsync(GetClaimsQuery query, CancellationToken ct)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();

    IQueryable<DbClaim> claims = await _accessValidator.IsAdminAsync(senderId)
      ? _provider.Claims.AsNoTracking()
      : _provider.Claims.AsNoTracking().Where(c => c.CreatedBy == senderId);

    if (!string.IsNullOrWhiteSpace(query.NameIncludeSubstring))
    {
      claims = claims.Where(c =>
        c.Content.Contains(query.NameIncludeSubstring) ||
        c.Name.Contains(query.NameIncludeSubstring));
    }

    if (query.Priority.HasValue)
    {
      claims = claims.Where(c => c.Priority == (int)query.Priority.Value);
    }

    if (query.Status.HasValue)
    {
      claims = claims.Where(c => c.Status == (int)query.Status.Value);
    }

    if (query.DeadLine.HasValue)
    {
      claims = claims.Where(c => c.DeadLine == query.DeadLine.Value);
    }

    if (query.CreatedBy.HasValue)
    {
      claims = claims.Where(c => c.CreatedBy == query.CreatedBy.Value);
    }

    if (query.IsAscendingSort.HasValue)
    {
      claims = query.IsAscendingSort.Value
        ? claims.OrderBy(c => c.Id)
        : claims.OrderByDescending(c => c.Id);
    }

    return (
      await claims
        .Skip(query.SkipCount)
        .Take(query.TakeCount)
        .ToListAsync(ct),
      await claims.CountAsync(ct));
  }
}
