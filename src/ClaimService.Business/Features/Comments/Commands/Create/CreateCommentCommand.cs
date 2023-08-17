using MediatR;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Commands.Create;

public record CreateCommentCommand : IRequest<Unit>
{
  public Guid ClaimId { get; set; }
  public CreateCommentRequest Request { get; set; }
}
