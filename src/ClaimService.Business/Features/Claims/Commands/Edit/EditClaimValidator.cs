using FluentValidation;
using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;

public class EditClaimValidator : BaseEditRequestValidator<EditClaimRequest>, IEditClaimValidator
{
  private readonly IDataProvider _provider;
  private readonly IDepartmentService _departmentService;
  private readonly IUserService _userService;
  private readonly IProjectService _projectService;
  private readonly IHttpContextAccessor _httpContextAccessor;

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
      nameof(EditClaimRequest.CategoryId),
      nameof(EditClaimRequest.DepartmentId),
      nameof(EditClaimRequest.Priority),
      nameof(EditClaimRequest.Status),
      nameof(EditClaimRequest.Deadline),
    });

    AddСorrectOperations(nameof(EditClaimRequest.Name), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.Content), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.CategoryId), new() { OperationType.Replace });
    AddСorrectOperations(nameof(EditClaimRequest.DepartmentId), new() { OperationType.Replace });
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
        { x => x.value.ToString().Trim().Length < 51, "Name is too long." }
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
          x => x.value is null || x.value?.ToString().Trim().Length < 501,
          "Description is too long."
        }
      });

    #endregion

    #region CategoryId

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.CategoryId),
      x => x == OperationType.Replace,
      new()
      {
        {
          async (x) => x.value is not null && Guid.TryParse(x.value.ToString().Trim(), out Guid categoryId) &&
            !await _provider.Categories.AnyAsync(c => c.Id == categoryId && c.IsActive),
          "Incorrect category id value."
        }
      });

    #endregion

    #region DepartmentId

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.DepartmentId),
      x => x == OperationType.Replace,
      new()
      {
        {
          async (x) => x.value is not null && Guid.TryParse(x.value.ToString().Trim(), out Guid departmentId) &&
            !await _departmentService.DoesDepartmentExist(new List<Guid> { departmentId }),
          "Incorrect department id value."
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
        },
        {
          x => DateTime.Parse(x.value?.ToString().Trim()) > DateTime.UtcNow,
          "Deadline value must be after current time."
        }
      });

    #endregion

    #region ResponsibleUserId

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.ResponsibleUserId),
      x => x == OperationType.Replace,
      new()
      {
        {
          async (x) => x.value is not null && Guid.TryParse(x.value.ToString().Trim(), out Guid userId) &&
            !await _userService.DoesUserExist(userId),
          "Incorrect responsible user id value."
        }
      });

    #endregion

    #region ManagerUserId

    await AddFailureForPropertyIfAsync(
      nameof(EditClaimRequest.ManagerUserId),
      x => x == OperationType.Replace,
      new()
      {
        {
          async (x) => x.value is not null && Guid.TryParse(x.value.ToString().Trim(), out Guid departmentId) &&
            !(await _departmentService.GetDepartmentManagersByUserId(_httpContextAccessor.HttpContext.GetUserId())).Union(
              await _projectService.GetProjectManagersByUserId(_httpContextAccessor.HttpContext.GetUserId()))
            .Any(id => id == departmentId),
          "Incorrect manager user id value."
        }
      });

    #endregion
  }

  public EditClaimValidator(
    IDataProvider provider,
    IDepartmentService departmentService,
    IUserService userService,
    IProjectService projectService,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _departmentService = departmentService;
    _userService = userService;
    _projectService = projectService;
    _httpContextAccessor = httpContextAccessor;

    RuleForEach(x => x.Operations)
      .CustomAsync(HandleInternalPropertyValidation);
  }
}
