using FluentValidation;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;

public class EditClaimValidator : BaseEditRequestValidator<EditClaimRequest>, IEditClaimValidator
{
  private async Task HandleInternalPropertyValidation(
    Operation<EditClaimRequest> requestedOperation,
    ValidationContext<JsonPatchDocument<EditClaimRequest>> context,
    CancellationToken cancellationToken)
  {
    RequestedOperation = requestedOperation;
    Context = context;

    #region Paths

    AddСorrectPaths(new()
    {
      nameof(EditClaimRequest.Name),
      nameof(EditClaimRequest.Content),
      nameof(EditClaimRequest.Priority),
      nameof(EditClaimRequest.Status),
      nameof(EditClaimRequest.Deadline),
    });

    AddСorrectOperations(nameof(EditClaimRequest.Name), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Content), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Priority), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Status), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Deadline), new() { OperationType.Replace });

    #endregion

    #region Name

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Name),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrWhiteSpace(x.value.ToString().Trim()), "Name can't be null or empty." },
        { x => x.value.ToString().Trim().Length < 301, "Name is too long." }
      },
      CascadeMode.Stop);

    #endregion

    #region Content

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Content),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => x.value is null || x.value.ToString().Trim().Length < 1001,
          "Description is too long."
        }
      });

    #endregion

    #region Priority

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.Priority),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => Task.FromResult(x.value is null || !Enum.TryParse(x.value.ToString().Trim(), true, out ClaimPriority priority)),
          "Incorrect claim priority value."
        }
      },
      CascadeMode.Stop);

    #endregion

    #region Status

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.Status),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => Task.FromResult(x.value is null || !Enum.TryParse(x.value.ToString().Trim(), true, out ClaimStatus priority)),
          "Incorrect claim status value."
        }
      },
      CascadeMode.Stop);

    #endregion

    #region Deadline

    AddFailureForPropertyIf(
      nameof(EditClaimRequest.Deadline),
      x => x == OperationType.Replace,
      new()
      {
        {
          x => DateTime.TryParse(x.value?.ToString().Trim(), out DateTime deadline),
          "Incorrect is active value."
        }
      });

    #endregion
  }

  public EditClaimValidator()
  {
    RuleForEach(x => x.Operations)
      .CustomAsync(HandleInternalPropertyValidation);
  }
}
