using FluentValidation;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;

[AutoInject]
public interface ICreateClaimRequestValidator : IValidator<CreateClaimRequest>
{
}
