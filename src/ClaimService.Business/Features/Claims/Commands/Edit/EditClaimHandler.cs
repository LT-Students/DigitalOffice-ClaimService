using FluentValidation.Results;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;

public class EditClaimHandler : IRequestHandler<EditClaimCommand, Unit>
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IAccessValidator _accessValidator;
  private readonly IEditClaimValidator _validator;

  public EditClaimHandler(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor,
    IAccessValidator accessValidator,
    IEditClaimValidator validator)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
    _accessValidator = accessValidator;
    _validator = validator;
  }

  public async Task<Unit> Handle(EditClaimCommand command, CancellationToken ct)
  {
    DbClaim claim = await _provider.Claims.FirstOrDefaultAsync(c => c.Id == command.ClaimId && c.Status != (int)ClaimStatus.Closed, ct);
    if (claim is null)
    {
      throw new BadRequestException("No claim with provided id was found");
    }

    Guid editorId = _httpContextAccessor.HttpContext.GetUserId();
    if (claim.CreatedBy != editorId && !await _accessValidator.IsAdminAsync(editorId))
    {
      throw new ForbiddenException("Not enough rights to edit claim.");
    }

    ValidationResult validationResult = await _validator.ValidateAsync(command.Patch, ct);
    if (!validationResult.IsValid)
    {
      throw new BadRequestException(validationResult.Errors.Select(e => e.ErrorMessage));
    }

    Map(command.Patch).ApplyTo(claim);
    claim.ModifiedBy = editorId;
    claim.ModifiedAtUtc = DateTime.UtcNow;
    await _provider.SaveAsync();

    return Unit.Value;
  }

  private JsonPatchDocument<DbClaim> Map(JsonPatchDocument<EditClaimRequest> patch)
  {
    JsonPatchDocument<DbClaim> claimPatch = new();

    foreach (Operation<EditClaimRequest> item in patch.Operations)
    {
      claimPatch.Operations.Add(new Operation<DbClaim>(item.op, item.path, item.from, item.value?.ToString().Trim()));
    }

    return claimPatch;
  }
}
