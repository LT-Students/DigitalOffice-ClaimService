using DigitalOffice.Kernel.OpenApi.OperationFilters;
using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Update;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaim;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaims;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
public class ClaimsController : ControllerBase
{
  private readonly IMediator _mediator;

  public ClaimsController(IMediator mediator)
  {
    _mediator = mediator;
  }

  /// <summary>
  /// Create claim
  /// </summary>
  /// <param name="command">Claim to create</param>
  [HttpPost]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> CreateAsync(
    [FromBody] CreateClaimCommand command,
    CancellationToken ct)
  {
    return Created("/claims", await _mediator.Send(command, ct));
  }

  /// <summary>
  /// Edit claim
  /// </summary>
  /// <param name="claimId">Id of the claim to edit.</param>
  /// <param name="patch">Properties of the claim to edit.</param>
  [HttpPatch("{claimId}")]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> EditAsync(
    [FromRoute] Guid claimId,
    [FromBody] JsonPatchDocument<EditClaimRequest> patch,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(new EditClaimCommand { ClaimId = claimId, Patch = patch }, ct));
  }

  /// <summary>
  /// Get claims
  /// </summary>
  /// <param name="query">Filtering and ordering parameters</param>
  [HttpGet]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(FindResult<ClaimInfo>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAsync(
    [FromQuery] GetClaimsQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }

  /// <summary>
  /// Get claim
  /// </summary>
  [HttpGet("{claimId}")]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(ClaimResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetAsync(
    [FromRoute] GetClaimQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }

  /// <summary>
  /// Update claim
  /// </summary>
  /// <param name="claimId">Id of claim to update</param>
  /// <param name="request">Properties of claim to update</param>
  [HttpPut("{claimId}")]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> UpdateAsync(
    [Required][FromRoute] Guid claimId,
    [Required] UpdateClaimRequest request,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(new UpdateClaimCommand
    {
      ClaimId = claimId,
      Request = request
    }, ct));
  }
}
