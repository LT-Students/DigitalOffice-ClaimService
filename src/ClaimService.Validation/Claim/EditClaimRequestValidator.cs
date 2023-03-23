using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class EditClaimRequestValidator : ExtendedEditRequestValidator<Guid, EditClaimRequest>, IEditClaimRequestValidator
{
  private readonly ICategoryRepository _categoryRepository;
  private async Task HandleInternalPropertyValidationAsync(
    Operation<EditClaimRequest> requestedOperation,
    ValidationContext<(Guid, JsonPatchDocument<EditClaimRequest>)> context)
  {
    Context = context;
    RequestedOperation = requestedOperation;

    AddСorrectPaths(
      new List<string>
      {
        nameof(EditClaimRequest.Name),
        nameof(EditClaimRequest.Content),
        nameof(EditClaimRequest.CategoryId),
        nameof(EditClaimRequest.Urgency),
        nameof(EditClaimRequest.Status),
        nameof(EditClaimRequest.Deadline)
      });

    AddСorrectOperations(nameof(EditClaimRequest.Name), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Content), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.CategoryId), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Urgency), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Status), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Deadline), new List<OperationType> { OperationType.Replace });

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Name),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), ClaimRequestValidatorResource.NotEmptyName },
        { x => x.value.ToString().Trim().Length < 100, ClaimRequestValidatorResource.TooLongName },
        { x => char.IsLetterOrDigit(x.value.ToString().Trim()[0]), ClaimRequestValidatorResource.FirstCharacterIsNotSpecialName }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Content),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), ClaimRequestValidatorResource.NotEmptyContent },
        { x => x.value.ToString().Trim().Length < 2000, ClaimRequestValidatorResource.TooLongContent}
      },
      CascadeMode.Stop);

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.CategoryId),
      x => x == OperationType.Replace,
      new()
      {
        { async (x) =>
          {
            return Guid.TryParse(x.value.ToString(), out Guid categoryId)
              ? await _categoryRepository.DoesExistAsync(categoryId)
              : false;
          },
          ClaimRequestValidatorResource.NotExistingCategoryId }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Status),
      x => x == OperationType.Replace,
      new()
      {
        { x => Enum.TryParse(typeof(ClaimStatus), x.value?.ToString(), out _), ClaimRequestValidatorResource.IncorrectClaimStatus }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Urgency),
      x => x == OperationType.Replace,
      new()
      {
        { x => Enum.TryParse(typeof(ClaimUrgency), x.value?.ToString(), out _), ClaimRequestValidatorResource.IncorrectClaimUrgency }
      },
      CascadeMode.Stop);

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Deadline),
      x => x == OperationType.Replace,
      new()
      {
        { x => string.IsNullOrEmpty(x.value?.ToString())
          ? true
          : DateTime.TryParse(x.value.ToString(), out DateTime result), ClaimRequestValidatorResource.IncorectDeadLineFormat}
      },
      CascadeMode.Stop);

  }
  public EditClaimRequestValidator(ICategoryRepository categoryRepository)
  {
    _categoryRepository = categoryRepository;

    RuleFor(paths => paths)
        .CustomAsync(async (paths, context, _) =>
        {
          foreach (var op in paths.Item2.Operations)
          {
            await HandleInternalPropertyValidationAsync(op, context);
          }
        });
  }
}
