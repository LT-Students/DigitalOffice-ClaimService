using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaim;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Queries.GetClaims;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
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
  /// Create claim.
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync(
    [FromBody] CreateClaimCommand command,
    CancellationToken ct)
  {
    return Created("/claims", await _mediator.Send(command, ct));
  }

  /// <summary>
  /// Edit claim.
  /// </summary>
  /// <param name="claimId">Id of the claim to edit.</param>
  /// <param name="patch">Properties of the claim to edit.</param>
  /// <returns></returns>
  [HttpPatch("{claimId}")]
  public async Task<IActionResult> EditAsync(
    [FromRoute] Guid claimId,
    [FromBody] JsonPatchDocument<EditClaimRequest> patch,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(new EditClaimCommand { ClaimId = claimId, Patch = patch }, ct));
  }

  /// <summary>
  /// Get claims.
  /// </summary>
  [HttpGet]
  public async Task<IActionResult> GetAsync(
    [FromQuery] GetClaimsQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }

  /// <summary>
  /// Get claim.
  /// </summary>
  [HttpGet("{claimId}")]
  public async Task<IActionResult> GetAsync(
    [FromRoute] GetClaimQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }
}
