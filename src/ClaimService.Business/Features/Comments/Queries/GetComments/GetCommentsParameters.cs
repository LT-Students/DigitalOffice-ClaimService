using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Queries.GetComments;

public record GetCommentsParameters : BaseFindFilter
{
  /// <summary>
  /// Sorting by time of creating
  /// </summary>
  [FromQuery(Name = "IsAscendingSort")]
  public bool? IsAscendingSort { get; set; }
}
