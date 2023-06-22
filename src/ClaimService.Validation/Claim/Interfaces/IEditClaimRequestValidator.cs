using FluentValidation;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;

[AutoInject]
public interface IEditClaimRequestValidator : IValidator<(Guid, JsonPatchDocument<EditClaimRequest>)>
{
}
