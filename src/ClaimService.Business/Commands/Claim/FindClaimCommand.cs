using LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim;

public class FindClaimCommand : IFindClaimCommand
{
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IClaimRepository _repository;
  private readonly IClaimInfoMapper _mapper;

  public FindClaimCommand(
    IHttpContextAccessor contextAccessor,
    IClaimRepository repository,
    IClaimInfoMapper mapper)
  {
    _contextAccessor = contextAccessor;
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<FindResultResponse<ClaimInfo>> ExecuteAsync(FindClaimFilter filter, CancellationToken ct)
  {
    Guid senderId = _contextAccessor.HttpContext.GetUserId();
    (List<DbClaim> dbClaims, int totalCount) = await _repository.FindAsync(filter, senderId, ct);

    return new FindResultResponse<ClaimInfo>(
      body: dbClaims.ConvertAll(_mapper.Map),
      totalCount: totalCount);
  }
}
