using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;

[AutoInject]
public interface IEditClaimCommand
{
  Task<OperationResultResponse<ClaimInfo>> ExecuteAsync(
    Guid claimId,
    JsonPatchDocument<EditClaimRequest> patch,
    CancellationToken cancellationToken);
}
