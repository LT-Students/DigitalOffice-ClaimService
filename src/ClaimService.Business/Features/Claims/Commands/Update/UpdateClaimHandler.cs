using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Update;

public class UpdateClaimHandler : IRequestHandler<UpdateClaimCommand, Unit>
{
  private readonly IDataProvider _provider;
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UpdateClaimHandler(
    IDataProvider provider,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<Unit> Handle(UpdateClaimCommand command, CancellationToken ct)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();
    // TODO: add right check.
    if (!await _accessValidator.IsAdminAsync(senderId))
    {
      throw new ForbiddenException("Not enough rights to edit claim category.");
    }

    DbClaim claim = await _provider.Claims.FirstAsync(c => c.Id == command.ClaimId, ct);
    UpdateClaimRequest request = command.Request;

    claim.Name = request.Name;
    claim.Content = request.Content;
    claim.CategoryId = request.CategoryId;
    claim.DepartmentId = request.DepartmentId;
    claim.Priority = (int)request.Priority;
    claim.DeadLine = request.Deadline;
    claim.ResponsibleUserId = request.ResponsibleUserId;
    claim.ManagerUserId = request.ManagerUserId;
    claim.ModifiedBy = senderId;
    claim.ModifiedAtUtc = DateTime.UtcNow;

    await _provider.SaveAsync();

    return Unit.Value;
  }
}
