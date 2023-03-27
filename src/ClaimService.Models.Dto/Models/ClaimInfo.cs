using System;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Models;

public record ClaimInfo
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Content { get; set; }
  public Guid CategoryId { get; set; }
  public ClaimStatus Status { get; set; }
  public ClaimPriority Priority { get; set; }
  public DateTime? DeadLine { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
}
