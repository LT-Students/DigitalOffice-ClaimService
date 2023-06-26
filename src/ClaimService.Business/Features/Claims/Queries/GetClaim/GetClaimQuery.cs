using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaim;

public record GetClaimQuery : IRequest<ClaimResponse>
{
  /// <summary>
  /// Id of the claim to get.
  /// </summary>
  [FromRoute(Name = "claimId")]
  public Guid ClaimId { get; set; }
}
