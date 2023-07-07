using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.Kernel.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery : BaseFindFilter, IRequest<FindResult<CategoryInfo>>
{
  /// <summary>
  /// Flag how to sort the results.
  /// </summary>
  [FromQuery(Name = "IsAscendingSort")]
  public bool? IsAscendingSort { get; set; }

  /// <summary>
  /// Flag whether to include deactivated categories.
  /// </summary>
  [FromQuery(Name = "IncludeDeactivated")]
  public bool IncludeDeactivated { get; set; }

  /// <summary>
  /// String that category name must contain.
  /// </summary>
  [FromQuery(Name = "NameIncludeSubstring")]
  public string NameIncludeSubstring { get; set; }

  /// <summary>
  /// Color that categories must have.
  /// </summary>
  [FromQuery(Name = "Color")]
  public Color? Color { get; set; }
}
