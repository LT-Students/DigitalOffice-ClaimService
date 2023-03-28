using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.ClaimService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;

[AutoInject]
public interface IGetClaimCommand
{
  Task<OperationResultResponse<ClaimResponse>> ExecuteAsync(GetClaimFilter filter, CancellationToken cancellationToken = default);
}
