using FluentValidation;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
  public CreateCategoryValidator()
  {
    RuleFor(r => r.Color)
      .IsInEnum()
      .WithMessage("No such Color.");
  }
}
