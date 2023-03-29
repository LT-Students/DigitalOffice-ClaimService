using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;

public record CreateClaimRequest
{
  [Required]
  public string Name { get; set; }
  [Required]
  public string Content { get; set; }
  public Guid CategoryId { get; set; }
  [DefaultValue(ClaimPriority.Magor)]
  public ClaimPriority? Priority { get; set; }
  public DateTime? Deadline { get; set; }
}
