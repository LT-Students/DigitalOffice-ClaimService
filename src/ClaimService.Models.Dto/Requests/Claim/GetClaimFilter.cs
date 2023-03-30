using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;

public record GetClaimFilter
{
  [FromQuery(Name = "claimId")]
  public Guid ClaimId { get; set; }
}
