using FluentValidation;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;

public class EditCategoryValidator : BaseEditRequestValidator<EditCategoryRequest>, IEditCategoryValidator
{
  private async Task HandleInternalPropertyValidation(
    Operation<EditCategoryRequest> requestedOperation,
    ValidationContext<JsonPatchDocument<EditCategoryRequest>> context,
    CancellationToken cancellationToken)
  {
    RequestedOperation = requestedOperation;
    Context = context;

    #region Paths

    AddСorrectPaths(new()
    {
      nameof(EditCategoryRequest.Name),
      nameof(EditCategoryRequest.Color)
    });

    AddСorrectOperations(nameof(EditCategoryRequest.Name), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditCategoryRequest.Color), new() { OperationType.Replace });

    #endregion

    #region Name

    AddFailureForPropertyIf(
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

    AddFailureForPropertyIf(
      nameof(EditCategoryRequest.Color),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => x.value is null || !Enum.TryParse(x.value.ToString().Trim(), true, out Color color),
          "Incorrect color value."
        }
      },
      CascadeMode.Stop);

    #endregion
  }

  public EditCategoryValidator()
  {
    RuleForEach(x => x.Operations)
      .CustomAsync(HandleInternalPropertyValidation);
  }
}
