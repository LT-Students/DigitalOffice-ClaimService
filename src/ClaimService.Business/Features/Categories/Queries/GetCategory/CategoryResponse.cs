using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;

public class CategoryResponse
{
  /// <summary>
  /// Category id
  /// </summary>
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Category name
  /// </summary>
  [Required]
  public string Name { get; set; }

  /// <summary>
  /// Category color
  /// </summary>
  public Color Color { get; set; }

  /// <summary>
  /// Whether category is archived or not
  /// </summary>
  [Required]
  public bool IsActive { get; set; }
}
