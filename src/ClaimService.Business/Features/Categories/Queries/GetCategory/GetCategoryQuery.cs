using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;

public record GetCategoryQuery : IRequest<CategoryResponse>
{
  /// <summary>
  /// Id of the category to get.
  /// </summary>
  [FromRoute(Name = "categoryId")]
  public Guid CategoryId { get; set; }
}
