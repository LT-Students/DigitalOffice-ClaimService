using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Models;

public class ClaimInfo
{
  /// <summary>
  /// Claim id
  /// </summary>
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Claim name
  /// </summary>
  [Required]
  public string Name { get; set; }

  /// <summary>
  /// Claim content
  /// </summary>
  [Required]
  public string Content { get; set; }

  /// <summary>
  /// Claim category id
  /// </summary>
  public Guid? CategoryId { get; set; }

  /// <summary>
  /// Claim Status
  /// </summary>
  [Required]
  public ClaimStatus Status { get; set; }

  /// <summary>
  /// Claim priority
  /// </summary>
  [Required]
  public ClaimPriority Priority { get; set; }

  /// <summary>
  /// Time claim must be closed before
  /// </summary>
  public DateTime? DeadLine { get; set; }

  /// <summary>
  /// Whether claim is archived or not
  /// </summary>
  [Required]
  public bool IsActive { get; set; }

  /// <summary>
  /// Who created claim
  /// </summary>
  [Required]
  public Guid CreatedBy { get; set; }

  /// <summary>
  /// Time when claim was created
  /// </summary>
  [Required]
  public DateTime CreatedAtUtc { get; set; }

  /// <summary>
  /// Who modified claim last time
  /// </summary>
  public Guid? ModifiedBy { get; set; }

  /// <summary>
  /// Time when claim was modified last time
  /// </summary>
  public DateTime? ModifiedAtUtc { get; set; }
}
