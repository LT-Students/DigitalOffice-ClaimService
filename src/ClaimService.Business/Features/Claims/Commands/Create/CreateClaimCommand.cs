using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using MediatR;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;

public record CreateClaimCommand : IRequest<Guid>
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
  /// Priority of the claim.
  /// </summary>
  [DefaultValue(ClaimPriority.Normal)]
  public ClaimPriority Priority { get; set; } = ClaimPriority.Normal;

  /// <summary>
  /// Deadline of the claim.
  /// </summary>
  public DateTime? Deadline { get; set; }
}
