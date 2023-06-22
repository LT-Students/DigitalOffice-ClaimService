using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;

public record CreateClaimRequest
{
  [Required]
  public string Name { get; set; }
  [Required]
  public string Content { get; set; }
  public Guid CategoryId { get; set; }
  public ClaimPriority Priority { get; set; } = ClaimPriority.Major;
  public DateTime? Deadline { get; set; }
}
