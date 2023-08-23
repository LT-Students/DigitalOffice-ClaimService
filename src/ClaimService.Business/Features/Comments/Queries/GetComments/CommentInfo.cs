using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Queries.GetComments;

public class CommentInfo
{
  /// <summary>
  /// Comment id
  /// </summary>
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Claim id
  /// </summary>
  [Required]
  public Guid ClaimId { get; set; }

  /// <summary>
  /// Comment content
  /// </summary>
  public string Content { get; set; }

  /// <summary>
  /// Time when comment was created
  /// </summary>
  [Required]
  public DateTime CreatedAtUtc { get; set; }

  /// <summary>
  /// Who created the comment
  /// </summary>
  [Required]
  public Guid CreatedBy { get; set; }

  /// <summary>
  /// Time when comment was modified last time
  /// </summary>
  public DateTime? ModifiedAtUtc { get; set; }

  /// <summary>
  /// Who modified the comment
  /// </summary>
  public Guid? ModifiedBy { get; set; }
}
