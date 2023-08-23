using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Commands.Create;

public record CreateCommentRequest
{
  /// <summary>
  /// Content of comment
  /// </summary>
  [FromBody]
  [Required]
  [MaxLength(500, ErrorMessage = "Content of the comment must be provided.")]
  public string Content { get; set; }
}
