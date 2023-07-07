using LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaim;

public class ClaimResponse
{
  [Required]
  public Guid Id { get; set; }
  [Required]
  public string Name { get; set; }
  [Required]
  public string Content { get; set; }
  public Guid? CategoryId { get; set; }
  [Required]
  public ClaimStatus Status { get; set; }
  [Required]
  public ClaimPriority Priority { get; set; }
  public DateTime? DeadLine { get; set; }
  [Required]
  public bool IsActive { get; set; }
  [Required]
  public Guid CreatedBy { get; set; }
  [Required]
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public CategoryResponse Category { get; set; }
}
