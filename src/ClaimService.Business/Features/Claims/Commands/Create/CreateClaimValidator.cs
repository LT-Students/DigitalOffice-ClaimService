using FluentValidation;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;
using System;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class CreateClaimValidator : AbstractValidator<CreateClaimCommand>
{
  public CreateClaimValidator()
  {
    RuleFor(r => r.Priority)
      .IsInEnum()
      .WithMessage("No such Priority.");

    When(request => request.Deadline.HasValue, () =>
    {
      RuleFor(r => r.Deadline)
        .Must(d => d > DateTime.UtcNow)
        .WithMessage("DeadLine must not be earlier than now.");
    });
  }
}
