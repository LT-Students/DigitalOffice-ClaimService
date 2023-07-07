using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;

public class CategoryResponse
{
  [Required]
  public Guid Id { get; set; }
  [Required]
  public string Name { get; set; }
  public Color Color { get; set; }
  [Required]
  public bool IsActive { get; set; }
}
