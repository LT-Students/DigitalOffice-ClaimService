using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;

public record EditClaimRequest
{
  public string Name { get; set; }
  public string Content { get; set; }
  public Guid CategoryId { get; set; }
  public ClaimPriority? Priority { get; set; }
  public ClaimStatus? Status { get; set; }
  public DateTime? Deadline { get; set; }
}
