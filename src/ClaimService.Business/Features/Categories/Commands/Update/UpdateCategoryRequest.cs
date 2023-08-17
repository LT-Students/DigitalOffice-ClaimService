using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Update;

public record UpdateCategoryRequest
{
  /// <summary>
  /// Name of the category.
  /// </summary>
  [MinLength(1, ErrorMessage = "Name must be not empty.")]
  [MaxLength(20, ErrorMessage = "Name must be shorter than 20 symbols.")]
  [Required]
  public string Name { get; set; }

  /// <summary>
  /// Color of the category.
  /// </summary>
  [Required]
  public Color Color { get; set; }
}
