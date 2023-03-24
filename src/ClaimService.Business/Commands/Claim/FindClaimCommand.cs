using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
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
  private readonly IAccessValidator _accessValidator;

  public FindClaimCommand(
    IHttpContextAccessor contextAccessor,
    IClaimRepository repository,
    IClaimInfoMapper mapper,
    IResponseCreator responseCreator,
    IBaseFindFilterValidator findFilterValidator,
    IAccessValidator accessValidator)
  {
    _contextAccessor = contextAccessor;
    _repository = repository;
    _mapper = mapper;
    _responseCreator = responseCreator;
    _baseFindValidator = findFilterValidator;
    _accessValidator = accessValidator;
  }

  public async Task<FindResultResponse<ClaimInfo>> ExecuteAsync(FindClaimFilter filter, CancellationToken cancellationToken)
  {
    Guid senderId = _contextAccessor.HttpContext.GetUserId();

    if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
    {
      return _responseCreator.CreateFailureFindResponse<ClaimInfo>(
        HttpStatusCode.BadRequest, errors);
    }

    (List<DbClaim> dbClaims, int totalcount) = await _repository.FindAsync(filter, cancellationToken);

    if (!await _accessValidator.IsAdminAsync(senderId))
    {
      dbClaims = dbClaims.Where(c => c.CreatedBy == senderId).ToList();
    }

    if (dbClaims is null || !dbClaims.Any())
    {
      return new FindResultResponse<ClaimInfo>(body: new List<ClaimInfo>(), errors: errors);
    }

    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

    return new FindResultResponse<ClaimInfo>(
      totalCount: totalcount,
      body: _mapper.Map(dbClaims));
  }
}
