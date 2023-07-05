using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Models;

public class ClaimInfo
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Content { get; set; }
  public Guid? CategoryId { get; set; }
  public ClaimStatus Status { get; set; }
  public ClaimPriority Priority { get; set; }
  public DateTime? DeadLine { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
}
