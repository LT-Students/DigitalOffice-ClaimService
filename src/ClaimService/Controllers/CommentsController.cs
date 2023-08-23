using DigitalOffice.Kernel.OpenApi.OperationFilters;
using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.Business.Features.Comments.Commands.Create;
using LT.DigitalOffice.ClaimService.Business.Features.Comments.Queries.GetComments;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("Claims/{claimId}/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
  private readonly IMediator _mediator;

  public CommentsController(IMediator mediator)
  {
    _mediator = mediator;
  }

  /// <summary>
  /// Create comment
  /// </summary>
  /// <param name="claimId">Claim id comment will be attached to</param>
  /// <param name="request">Properties of comment</param>
  [HttpPost]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> CreateAsync(
    [FromRoute][Required] Guid claimId,
    [FromBody][Required] CreateCommentRequest request,
    CancellationToken ct)
  {
    return Created("/comments", await _mediator.Send(new CreateCommentCommand
    {
      ClaimId = claimId,
      Request = request
    }, ct));
  }

  /// <summary>
  /// Get comments
  /// </summary>
  /// <param name="claimId">Claim id to get comments of</param>
  /// <param name="parameters">Filtering and sorting parameters</param>
  [HttpGet]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(FindResult<CommentInfo>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetAsync(
    [FromRoute][Required] Guid claimId,
    [FromQuery] GetCommentsParameters parameters,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(new GetCommentsQuery
    {
      ClaimId = claimId,
      Parameters = parameters
    }, ct));
  }
}
