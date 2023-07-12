using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;

public class CreateClaimHandler : IRequestHandler<CreateClaimCommand, Guid>
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public CreateClaimHandler(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
  }

  public Task<Guid> Handle(CreateClaimCommand request, CancellationToken ct)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();

    return CreateAsync(Map(request, senderId));
  }

  private async Task<Guid> CreateAsync(DbClaim claim)
  {
    _provider.Claims.Add(claim);
    await _provider.SaveAsync();

    return claim.Id;
  }

  private DbClaim Map(CreateClaimCommand request, Guid senderId)
  {
    return new()
    {
      Id = Guid.NewGuid(),
      Name = request.Name,
      Content = request.Content,
      CategoryId = request.CategoryId,
      DepartmentId = request.DepartmentId,
      Status = (int)ClaimStatus.Created,
      Priority = (int)request.Priority,
      DeadLine = request.Deadline,
      ResponsibleUserId = request.ResponsibleUserId,
      ManagerUserId = request.ManagerUserId,
      IsActive = true,
      CreatedAtUtc = DateTime.UtcNow,
      CreatedBy = senderId
    };
  }
}
