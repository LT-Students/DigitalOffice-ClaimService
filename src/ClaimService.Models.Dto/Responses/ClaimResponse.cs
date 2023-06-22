using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Responses;

public class ClaimResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Content { get; set; }
  public Guid CategoryId { get; set; }
  public ClaimStatus Status { get; set; }
  public ClaimPriority Priority { get; set; }
  public DateTime? DeadLine { get; set; }
  public UserInfo CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
}
