using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.ClaimService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.ClaimService.Controllers;

[Route("[controller]")]
[ApiController]
public class ClaimController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid?>> CreateAsync(
    [FromServices] ICreateClaimCommand command,
    [FromBody] CreateClaimRequest request,
    CancellationToken cancellationToken)
  {
    return await command.ExecuteAsync(request, cancellationToken);
  }

  [HttpGet("find")]
  public async Task<FindResultResponse<ClaimInfo>> FindAsync(
    [FromServices] IFindClaimCommand command,
    [FromQuery] FindClaimFilter filter,
    CancellationToken cancellationToken)
  {
    return await command.ExecuteAsync(filter, cancellationToken);
  }

  [HttpGet("get")]
  public async Task<OperationResultResponse<ClaimResponse>> GetAsync(
    [FromServices] IGetClaimCommand command,
    [FromQuery] GetClaimFilter filter,
    CancellationToken cancellationToken)
  {
    return await command.ExecuteAsync(filter, cancellationToken);
  }

  [HttpPatch("edit")]
  public async Task<OperationResultResponse<ClaimInfo>> EditAsync(
    [FromServices] IEditClaimCommand command,
    [FromQuery] Guid claimId,
    [FromBody] JsonPatchDocument<EditClaimRequest> path,
    CancellationToken cancellationToken)
  {
    return await command.ExecuteAsync(claimId, path, cancellationToken);
  }
}
