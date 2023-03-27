using System;
using System.ComponentModel.DataAnnotations;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Requests;

public record CreateClaimRequest
{
  [Required]
  public string Name { get; set; }
  [Required]
  public string Content { get; set; }
  public Guid CategoryId { get; set; }
  public ClaimPriority? Priority { get; set; }
  public DateTime? Deadline { get; set; }

}
