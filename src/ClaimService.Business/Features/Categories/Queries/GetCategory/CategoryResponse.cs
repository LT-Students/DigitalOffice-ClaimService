using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;

public class CategoryResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public Color Color { get; set; }
  public bool IsActive { get; set; }
}
