using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Validation.Claim.Resourses;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class EditClaimRequestValidator : ExtendedEditRequestValidator<Guid, EditClaimRequest>, IEditClaimRequestValidator
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IClaimRepository _claimRepository;
  private readonly IHttpContextAccessor _contextAccessor;

  private async Task HandleInternalPropertyValidationAsync(
    Operation<EditClaimRequest> requestedOperation,
    ValidationContext<(Guid, JsonPatchDocument<EditClaimRequest>)> context,
    DbClaim dbClaim,
    Guid senderId)
  {
    Context = context;
    RequestedOperation = requestedOperation;

    AddСorrectPaths(
      new List<string>
      {
        nameof(EditClaimRequest.Name),
        nameof(EditClaimRequest.Content),
        nameof(EditClaimRequest.CategoryId),
        nameof(EditClaimRequest.Priority),
        nameof(EditClaimRequest.Status),
        nameof(EditClaimRequest.Deadline)
      });

    AddСorrectOperations(nameof(EditClaimRequest.Name), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Content), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.CategoryId), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Priority), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Status), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Deadline), new List<OperationType> { OperationType.Replace });

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Name),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), ClaimEditRequestValidatorRsourses.NotEmptyName },
        { x => x.value.ToString().Trim().Length < 100, ClaimEditRequestValidatorRsourses.TooLongName },
        { x => char.IsLetterOrDigit(x.value.ToString().Trim()[0]), ClaimEditRequestValidatorRsourses.FirstCharacterIsNotSpecialName }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Content),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), ClaimEditRequestValidatorRsourses.NotEmptyContent },
        { x => x.value.ToString().Trim().Length < 2000, ClaimEditRequestValidatorRsourses.TooLongContent }
      },
      CascadeMode.Stop);

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.CategoryId),
      x => x == OperationType.Replace,
      new()
      {
        {
          async (x) =>
          {
            return Guid.TryParse(x.value.ToString(), out Guid categoryId)
              ? await _categoryRepository.DoesExistAsync(categoryId)
                && (dbClaim.Status == ClaimStatus.New
                || dbClaim.Status == ClaimStatus.Created
                || dbClaim.Status == ClaimStatus.Denied)
              : false;
          },
          ClaimEditRequestValidatorRsourses.NotExistingCategoryId
        }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Status),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => Enum.TryParse(x.value?.ToString(), out ClaimStatus res)
          && Enum.IsDefined(typeof(ClaimStatus),res),
          ClaimEditRequestValidatorRsourses.IncorrectClaimStatus
        },
        { x => Enum.TryParse(x.value?.ToString(), out ClaimStatus res)
          && res == ClaimStatus.Closed || res == ClaimStatus.Returned ? dbClaim.CreatedBy == senderId : true,
          ClaimEditRequestValidatorRsourses.IncorrectUser
        }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Priority),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => Enum.TryParse(x.value?.ToString(), out ClaimPriority res)
          && Enum.IsDefined(typeof(ClaimPriority),res),
          ClaimEditRequestValidatorRsourses.IncorrectClaimPriority
        }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Deadline),
      x => x == OperationType.Replace,
      new()
      {
        { x => string.IsNullOrEmpty(x.value?.ToString())
          ? true
          : DateTime.TryParse(x.value.ToString(), out DateTime result)
          && result > DateTime.Now,
          ClaimEditRequestValidatorRsourses.IncorectDeadLineFormat
        }
      },
      CascadeMode.Stop);

  }
  public EditClaimRequestValidator(
    ICategoryRepository categoryRepository,
    IClaimRepository claimRepository,
    IHttpContextAccessor contextAccessor)
  {
    _categoryRepository = categoryRepository;
    _claimRepository = claimRepository;
    _contextAccessor = contextAccessor;

    Guid senderId = _contextAccessor.HttpContext.GetUserId();

    When(x => x.Item2.Operations.Any(), () =>
    {
      RuleFor(x => x.Item1)
        .MustAsync(async (x, _) => await _claimRepository.GetAsync(new() { Id = x }, senderId, _) is not null)
        .WithMessage("You have no rights to edit this claim.");
    });

    RuleFor(paths => paths)
        .CustomAsync(async (paths, context, _) =>
        {
          DbClaim dbClaim = await _claimRepository.GetAsync(new() { Id = paths.Item1 }, senderId, _);

          foreach (var op in paths.Item2.Operations)
          {
            await HandleInternalPropertyValidationAsync(op, context, dbClaim, senderId);
          }
        });
  }
}
