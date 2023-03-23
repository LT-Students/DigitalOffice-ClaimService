using System;
using FluentValidation;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class CreateClaimRequestValidator : AbstractValidator<CreateClaimRequest>, ICreateClaimRequestValidator
{
  public CreateClaimRequestValidator(ICategoryRepository categoryRepository)
  {
    RuleFor(request => request.Name)
      .MaximumLength(100)
      .WithMessage("Name must be shorter than 100 symbbolss.");

    RuleFor(request => request.Content)
      .MaximumLength(2000)
      .WithMessage("Content must be shorter than 2000 symbols.");

    RuleFor(request => request.CategoryId)
      .MustAsync(async (x, _) => await categoryRepository.DoesExistAsync(x))
      .WithMessage("No such Category.");

    RuleFor(request => request.Urgency)
      .IsInEnum()
      .WithMessage("No such Urgency");

    When(request => request.Deadline is not null,
      () =>
    {
      RuleFor(request => request.Deadline)
      .Must(x => x > DateTime.UtcNow)
      .WithMessage("DeadLine must not be earlier than now");
    });
  }
}
