using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;

public record CreateCategoryCommand : IRequest<Guid>
{
  /// <summary>
  /// Name of the category.
  /// </summary>
  [Required]
  [MaxLength(20, ErrorMessage = "Name must be shorter than 20 symbols.")]
  public string Name { get; set; }

  /// <summary>
  /// Color of the category.
  /// </summary>
  [Required]
  public Color Color { get; set; }
}
