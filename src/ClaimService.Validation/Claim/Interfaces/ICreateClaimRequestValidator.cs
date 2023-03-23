using FluentValidation;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;

[AutoInject]
public interface ICreateClaimRequestValidator : IValidator<CreateClaimRequest>
{
}
