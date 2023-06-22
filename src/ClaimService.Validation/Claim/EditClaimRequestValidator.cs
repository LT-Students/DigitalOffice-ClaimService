using FluentValidation;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;
using LT.DigitalOffice.ClaimService.Validation.Claim.Resourses;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class EditClaimRequestValidator : ExtendedEditRequestValidator<Guid, EditClaimRequest>, IEditClaimRequestValidator
{
  private readonly ICategoryRepository _categoryRepository;

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
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), EditClaimRequestValidatorResourses.NotEmptyName },
        { x => x.value.ToString().Trim().Length < 100, EditClaimRequestValidatorResourses.TooLongName },
        { x => char.IsLetterOrDigit(x.value.ToString().Trim()[0]), EditClaimRequestValidatorResourses.FirstCharacterIsNotSpecialName }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Content),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), EditClaimRequestValidatorResourses.NotEmptyContent },
        { x => x.value.ToString().Trim().Length < 2000, EditClaimRequestValidatorResourses.TooLongContent }
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
            return Guid.TryParse(x.value.ToString(), out Guid categoryId) &&
              await _categoryRepository.DoesExistAsync(categoryId) &&
              (dbClaim.Status == ClaimStatus.New ||
              dbClaim.Status == ClaimStatus.Created ||
              dbClaim.Status == ClaimStatus.Denied);
          },
          EditClaimRequestValidatorResourses.NotExistingCategoryId
        }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Status),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => Enum.TryParse(x.value?.ToString(), out ClaimStatus res) &&
          Enum.IsDefined(typeof(ClaimStatus),res),
          EditClaimRequestValidatorResourses.IncorrectClaimStatus
        },
        { x => ((!Enum.TryParse(x.value?.ToString(), out ClaimStatus res) ||
          res != ClaimStatus.Closed) && res != ClaimStatus.Returned) || dbClaim.CreatedBy == senderId,
          EditClaimRequestValidatorResourses.IncorrectUser
        }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Priority),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => Enum.TryParse(x.value?.ToString(), out ClaimPriority res) &&
          Enum.IsDefined(typeof(ClaimPriority), res),
          EditClaimRequestValidatorResourses.IncorrectClaimPriority
        }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Deadline),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => string.IsNullOrEmpty(x.value?.ToString()) ||
            (DateTime.TryParse(x.value.ToString(), out DateTime result) &&
            result > DateTime.Now),
          EditClaimRequestValidatorResourses.IncorectDeadLineFormat
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

    Guid senderId = contextAccessor.HttpContext.GetUserId();

    When(x => x.Item2.Operations.Any(), () =>
    {
      RuleFor(x => x.Item1)
        .MustAsync(async (x, _) => await claimRepository.GetAsync(new() { ClaimId = x }, senderId, _) is not null)
        .WithMessage("You have no rights to edit this claim.");
    });

    RuleFor(paths => paths)
      .CustomAsync(async (paths, context, _) =>
      {
        DbClaim dbClaim = await claimRepository.GetAsync(new() { ClaimId = paths.Item1 }, senderId, _);

        foreach (Operation<EditClaimRequest> op in paths.Item2.Operations)
        {
          await HandleInternalPropertyValidationAsync(op, context, dbClaim, senderId);
        }
      });
  }
}
