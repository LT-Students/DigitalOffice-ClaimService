using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;

[AutoInject]
public interface ICreateClaimCommand
{
  Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateClaimRequest request, CancellationToken cancellationToken);
}
