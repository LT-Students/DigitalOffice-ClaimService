using FluentValidation;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;
using LT.DigitalOffice.ClaimService.DataLayer;
using Microsoft.EntityFrameworkCore;
using System;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class CreateClaimValidator : AbstractValidator<CreateClaimCommand>
{
  public CreateClaimValidator(IDataProvider provider)
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

    RuleFor(r => r.CategoryId)
      .MustAsync((id, ct) => provider.Categories.AnyAsync(c => c.Id == id && c.IsActive, ct))
      .WithMessage("No category with provided id was found.");
  }
}
