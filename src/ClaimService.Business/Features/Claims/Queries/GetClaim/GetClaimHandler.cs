using LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaim;

public class GetClaimHandler : IRequestHandler<GetClaimQuery, ClaimResponse>
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IAccessValidator _accessValidator;

  public GetClaimHandler(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor,
    IAccessValidator accessValidator)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
    _accessValidator = accessValidator;
  }

  public async Task<ClaimResponse> Handle(GetClaimQuery query, CancellationToken ct)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();
    DbClaim claim = await GetAsync(query, ct);
    if (claim is null || (senderId != claim.CreatedBy && !await _accessValidator.IsAdminAsync()))
    {
      throw new NotFoundException("No claim was found.");
    }

    return Map(claim);
  }

  private Task<DbClaim> GetAsync(GetClaimQuery query, CancellationToken ct)
  {
    return _provider.Claims.AsNoTracking()
      .Include(c => c.Category)
      .Where(c => c.Id == query.ClaimId)
      .FirstOrDefaultAsync(ct);
  }

  private ClaimResponse Map(DbClaim claim)
  {
    return new()
    {
      Id = claim.Id,
      Name = claim.Name,
      Content = claim.Content,
      CategoryId = claim.CategoryId,
      Status = (ClaimStatus)claim.Status,
      Priority = (ClaimPriority)claim.Priority,
      DeadLine = claim.DeadLine,
      IsActive = claim.IsActive,
      CreatedBy = claim.CreatedBy,
      CreatedAtUtc = claim.CreatedAtUtc,
      Category = new CategoryResponse
      {
        Id = claim.Category.Id,
        Name = claim.Category.Name,
        Color = (Color)claim.Category.Color,
        IsActive = claim.Category.IsActive
      }
    };
  }
}
