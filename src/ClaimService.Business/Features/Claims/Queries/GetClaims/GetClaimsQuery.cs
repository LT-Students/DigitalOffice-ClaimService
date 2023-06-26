using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaims;

public record GetClaimsQuery : BaseFindFilter, IRequest<FindResult<ClaimInfo>>
{
  /// <summary>
  /// Flag how to sort the results.
  /// </summary>
  [FromQuery(Name = "IsAscendingSort")]
  public bool? IsAscendingSort { get; set; }

  /// <summary>
  /// String that claim name or content must contain.
  /// </summary>
  [FromQuery(Name = "NameIncludeSubstring")]
  public string NameIncludeSubstring { get; set; }

  /// <summary>
  /// Priority that claims must have.
  /// </summary>
  [FromQuery(Name = "Priority")]
  public ClaimPriority? Priority { get; set; }

  /// <summary>
  /// Status that claims must have.
  /// </summary>
  [FromQuery(Name = "Status")]
  public ClaimStatus? Status { get; set; }

  /// <summary>
  /// Maximum deadline that claims must be before of.
  /// </summary>
  [FromQuery(Name = "DeadLine")]
  public DateTime? DeadLine { get; set; }

  /// <summary>
  /// If of the user that claims must be created by.
  /// </summary>
  [FromQuery(Name = "CreatedBy")]
  public Guid? CreatedBy { get; set; }
}
