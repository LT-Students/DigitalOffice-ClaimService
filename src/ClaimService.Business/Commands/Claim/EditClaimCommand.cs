using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim;

public class EditClaimCommand : IEditClaimCommand
{
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IClaimRepository _repository;
  private readonly IResponseCreator _responseCreator;
  private readonly IPatchDbClaimMapper _mapper;
  private readonly IClaimInfoMapper _claimInfoMapper;
  private readonly IEditClaimRequestValidator _validator;

  public EditClaimCommand(
    IHttpContextAccessor contextAccessor,
    IClaimRepository repository,
    IResponseCreator responseCreator,
    IPatchDbClaimMapper mapper,
    IClaimInfoMapper claimInfoMapper,
    IEditClaimRequestValidator validator)
  {
    _contextAccessor = contextAccessor;
    _repository = repository;
    _responseCreator = responseCreator;
    _mapper = mapper;
    _claimInfoMapper = claimInfoMapper;
    _validator = validator;
  }

  public async Task<OperationResultResponse<ClaimInfo>> ExecuteAsync(
    Guid claimId,
    JsonPatchDocument<EditClaimRequest> patch,
    CancellationToken cancellationToken)
  {

    ValidationResult validationResult = await _validator.ValidateAsync((claimId, patch));
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<ClaimInfo>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.ConvertAll(e => e.ErrorMessage));
    }

    Guid senderId = _contextAccessor.HttpContext.GetUserId();
    DbClaim dbClaim = await _repository.EditAsync(claimId, _mapper.Map(patch), senderId, cancellationToken);

    if (dbClaim is null)
    {
      return _responseCreator.CreateFailureResponse<ClaimInfo>(HttpStatusCode.BadRequest);
    }

    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

    return new OperationResultResponse<ClaimInfo>(body: _claimInfoMapper.Map(dbClaim));
  }
}
