using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;

[AutoInject]
public interface IEditClaimValidator : IValidator<JsonPatchDocument<EditClaimRequest>>
{
}
