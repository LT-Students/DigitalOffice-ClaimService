using FluentValidation;
using LT.DigitalOffice.ClaimService.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Update;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
  public UpdateCategoryValidator(IDataProvider provider)
  {
    RuleFor(r => r.CategoryId)
      .NotEmpty()
      .WithMessage("Incorrect category id provided.")
      .MustAsync((id, ct) => provider.Categories.AnyAsync(c => c.Id == id && c.IsActive, ct))
      .WithErrorCode("404")
      .WithMessage("No category with provided id exist.");

    RuleFor(r => r.Request.Color)
      .IsInEnum()
      .WithMessage("No such color.");
  }
}
