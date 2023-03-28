using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.ClaimService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim;

public class GetClaimCommand : IGetClaimCommand
{
  private readonly IResponseCreator _responseCreator;
  private readonly IClaimRepository _repository;
  private readonly IClaimResponseMapper _mapper;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IUserService _userService;
  private readonly IUserInfoMapper _userInfoMapper;
  private readonly IAccessValidator _accessValidator;

  public GetClaimCommand(
    IResponseCreator responseCreator,
    IClaimRepository repository,
    IClaimResponseMapper mapper,
    IHttpContextAccessor contextAccessor,
    IUserService userService,
    IUserInfoMapper userInfoMapper,
    IAccessValidator accessValidator)
  {
    _responseCreator = responseCreator;
    _repository = repository;
    _mapper = mapper;
    _contextAccessor = contextAccessor;
    _userService = userService;
    _userInfoMapper = userInfoMapper;
    _accessValidator = accessValidator;
  }

  public async Task<OperationResultResponse<ClaimResponse>> ExecuteAsync(GetClaimFilter filter, CancellationToken cancellationToken = default)
  {
    Guid senderId = _contextAccessor.HttpContext.GetUserId();
    DbClaim dbClaim = await _repository.GetAsync(filter, cancellationToken);

    if (dbClaim is null)
    {
      return _responseCreator.CreateFailureResponse<ClaimResponse>(HttpStatusCode.NotFound);
    }

    if (!await _accessValidator.IsAdminAsync(senderId) && dbClaim.CreatedBy != senderId)
    {
      return _responseCreator.CreateFailureResponse<ClaimResponse>(HttpStatusCode.Forbidden);
    }

    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

    return new OperationResultResponse<ClaimResponse>(
      body: _mapper.Map(
        dbClaim,
        _userInfoMapper.Map((await _userService.GetUsersDataAsync(new List<Guid> { dbClaim.CreatedBy })).First())));
  }
}
