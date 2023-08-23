using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Update;

public record UpdateClaimRequest
{
  /// <summary>
  /// Name of the claim.
  /// </summary>
  [Required]
  [MaxLength(50, ErrorMessage = "Name must be shorter than 50 symbols.")]
  public string Name { get; set; }

  /// <summary>
  /// Content of the claim.
  /// </summary>
  [Required]
  [MaxLength(500, ErrorMessage = "Content must be shorter than 500 symbols.")]
  public string Content { get; set; }

  /// <summary>
  /// Id of category of the claim.
  /// </summary>
  public Guid? CategoryId { get; set; }

  /// <summary>
  /// Id of department the claim will be assigned to.
  /// </summary>
  public Guid? DepartmentId { get; set; }

  /// <summary>
  /// Priority of the claim.
  /// </summary>
  [Required]
  public ClaimPriority Priority { get; set; }

  /// <summary>
  /// Deadline of the claim.
  /// </summary>
  public DateTime? Deadline { get; set; }

  /// <summary>
  /// Id of user responsible for the claim.
  /// </summary>
  public Guid? ResponsibleUserId { get; set; }

  /// <summary>
  /// Claim creator's manager user id.
  /// </summary>
  [Required]
  public Guid ManagerUserId { get; set; }
}
