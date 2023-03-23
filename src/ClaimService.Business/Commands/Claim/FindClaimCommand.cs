using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Db.Intterfaces;
using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim;

public class FindClaimCommand : IFindClaimCommand
{
  private readonly IBaseFindFilterValidator _baseFindValidator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IClaimRepository _repository;
  private readonly IClaimInfoMapper _mapper;
  private readonly IResponseCreator _responseCreator;

  public FindClaimCommand (
    IHttpContextAccessor contextAccessor, 
    IClaimRepository repository,
    IClaimInfoMapper mapper, 
    IResponseCreator responseCreator,
    IBaseFindFilterValidator findFilterValidator)
  {
    _contextAccessor = contextAccessor;
    _repository = repository;
    _mapper = mapper;
    _responseCreator = responseCreator;
    _baseFindValidator = findFilterValidator;
  }

  public async Task<FindResultResponse<ClaimInfo>> ExecuteAsync (FindClaimFilter filter, CancellationToken cancellationToken)
  {
    if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
    {
      return _responseCreator.CreateFailureFindResponse<ClaimInfo>(
        HttpStatusCode.BadRequest, errors);
    }

    (List<DbClaim> dbClaims, int totalcount) = await _repository.FindAsync(filter, cancellationToken);

    if (dbClaims is null || !dbClaims.Any())
    {
      return new FindResultResponse<ClaimInfo> (body: new List<ClaimInfo>(), errors: errors);
    }

    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

    return new FindResultResponse<ClaimInfo>(
      totalCount: totalcount,
      body: _mapper.Map(dbClaims));
  }
}
