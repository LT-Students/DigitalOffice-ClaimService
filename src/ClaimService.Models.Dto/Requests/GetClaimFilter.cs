using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Requests;

public record GetClaimFilter
{
  [FromQuery(Name = "claimId")]
  public Guid Id { get; set; }
}
