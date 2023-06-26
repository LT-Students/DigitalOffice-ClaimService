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
  [MaxLength(100, ErrorMessage = "Name must be shorter than 100 symbols.")]
  public string Name { get; set; }

  /// <summary>
  /// Content of the claim.
  /// </summary>
  [Required]
  [MaxLength(2000, ErrorMessage = "Content must be shorter than 2000 symbols.")]
  public string Content { get; set; }

  /// <summary>
  /// Priority of the claim.
  /// </summary>
  [DefaultValue(ClaimPriority.Major)]
  public ClaimPriority Priority { get; set; } = ClaimPriority.Major;

  /// <summary>
  /// Deadline of the claim.
  /// </summary>
  public DateTime? Deadline { get; set; }
}
