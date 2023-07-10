using LT.DigitalOffice.ClaimService.Business.Shared.Enums;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;

public record EditCategoryRequest
{
  public string Name { get; set; }
  public Color? Color { get; set; }
}
