using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Mappers.Db.Intterfaces;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim;

public class CreateClaimCommand : ICreateClaimCommand
{
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IClaimRepository _repository;
  private readonly ICreateClaimRequestValidator _validator;
  private readonly IDbClaimMapper _mapper;
  private readonly IResponseCreator _responseCreator;

  public CreateClaimCommand(
    IHttpContextAccessor contextAccessor,
    IClaimRepository repository,
    ICreateClaimRequestValidator validator,
    IDbClaimMapper mapper,
    IResponseCreator responseCreator)
  {
    _contextAccessor = contextAccessor;
    _repository = repository;
    _validator = validator;
    _mapper = mapper;
    _responseCreator = responseCreator;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateClaimRequest request, CancellationToken cancellationToken)
  {
    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.ConvertAll(e => e.ErrorMessage));
    }

    Guid senderId = _contextAccessor.HttpContext.GetUserId();
    OperationResultResponse<Guid?> response = new(body: await _repository.CreateAsync(_mapper.Map(request, senderId)));

    _contextAccessor.HttpContext.Response.StatusCode = response.Body is null
      ? (int)HttpStatusCode.BadRequest
      : (int)HttpStatusCode.Created;

    return response;
  }
}
