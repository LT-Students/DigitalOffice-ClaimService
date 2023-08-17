using FluentValidation;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;

public class EditCategoryValidator : BaseEditRequestValidator<EditCategoryRequest>, IEditCategoryValidator
{
  private void HandleInternalPropertyValidation(
    Operation<EditCategoryRequest> requestedOperation,
    ValidationContext<JsonPatchDocument<EditCategoryRequest>> context)
  {
    RequestedOperation = requestedOperation;
    Context = context;

    #region Paths

    AddCorrectPaths(new()
    {
      nameof(EditCategoryRequest.Name),
      nameof(EditCategoryRequest.Color)
    });

    AddCorrectOperations(nameof(EditCategoryRequest.Name), new() { OperationType.Replace });
    AddCorrectOperations(nameof(EditCategoryRequest.Color), new() { OperationType.Replace });

    #endregion

    #region Name

    AddFailureForPropertyIfNot(
      nameof(EditCategoryRequest.Name),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrWhiteSpace(x.value.ToString().Trim()), "Name can't be null or empty." },
        { x => x.value.ToString().Trim().Length < 21, "Name is too long." }
      },
      CascadeMode.Stop);

    #endregion

    #region Color

    AddFailureForPropertyIfNot(
      nameof(EditCategoryRequest.Color),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => x.value is null || (Enum.TryParse(x.value.ToString().Trim(), true, out Color color) && Enum.IsDefined(color)),
          "Incorrect color value."
        }
      },
      CascadeMode.Stop);

    #endregion
  }

  public EditCategoryValidator()
  {
    RuleForEach(x => x.Operations)
      .Custom(HandleInternalPropertyValidation);
  }
}
