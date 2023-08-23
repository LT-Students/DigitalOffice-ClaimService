using FluentValidation;
using LT.DigitalOffice.ClaimService.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Commands.Create;

public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
{
  public CreateCommentValidator(IDataProvider provider)
  {
    ClassLevelCascadeMode = CascadeMode.Stop;

    RuleFor(r => r.ClaimId)
      .NotEmpty()
      .WithMessage("Incorrect claim id.")
      .MustAsync((id, ct) => provider.Claims.AnyAsync(c => c.Id == id && c.IsActive, ct))
      .WithErrorCode("404")
      .WithMessage("No claim with provided id was found.");
  }
}
