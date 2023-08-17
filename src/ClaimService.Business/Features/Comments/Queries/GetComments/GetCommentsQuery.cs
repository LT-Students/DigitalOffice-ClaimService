using DigitalOffice.Kernel.Responses;
using MediatR;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Queries.GetComments;

public record GetCommentsQuery : IRequest<FindResult<CommentInfo>>
{
  public Guid ClaimId { get; set; }
  public GetCommentsParameters Parameters { get; set; }
}
