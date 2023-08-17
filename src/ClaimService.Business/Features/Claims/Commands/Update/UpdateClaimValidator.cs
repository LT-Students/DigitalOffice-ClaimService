using FluentValidation;
using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Update;

public class UpdateClaimValidator : AbstractValidator<UpdateClaimCommand>
{
  public UpdateClaimValidator(
    IDataProvider provider,
    IDepartmentService departmentService,
    IUserService userService,
    IProjectService projectService,
    IHttpContextAccessor httpContextAccessor)
  {
    ClassLevelCascadeMode = CascadeMode.Stop;

    RuleFor(r => r.ClaimId)
      .NotEmpty()
      .WithMessage("Incorrect claim id provided.")
      .MustAsync((id, ct) => provider.Claims.AnyAsync(c => c.Id == id && c.IsActive, ct))
      .WithErrorCode("404")
      .WithMessage("No claim id with provided id was found.");

    When(r => r.Request.CategoryId.HasValue, () =>
    {
      RuleFor(r => r.Request.CategoryId.Value)
        .MustAsync((id, ct) => provider.Categories.AnyAsync(c => c.Id == id && c.IsActive, ct))
        .WithErrorCode("404")
        .WithMessage("No category with provided id was found");
    });

    When(r => r.Request.DepartmentId.HasValue, () =>
    {
      RuleFor(r => r.Request.DepartmentId.Value)
        .MustAsync((id, ct) => departmentService.DoesDepartmentExist(new List<Guid> { id }))
        .WithMessage("No department with provided id was found.");
    });

    RuleFor(r => r.Request.Priority)
      .IsInEnum()
      .WithMessage("No such Priority.");

    When(request => request.Request.Deadline.HasValue, () =>
    {
      RuleFor(r => r.Request.Deadline)
        .Must(d => d > DateTime.UtcNow)
        .WithMessage("DeadLine must not be earlier than now.");
    });

    When(r => r.Request.ResponsibleUserId.HasValue, () =>
    {
      RuleFor(r => r.Request.ResponsibleUserId.Value)
        .MustAsync((id, _) => userService.DoesUserExist(id))
        .WithMessage("User with provided id doesn't exist.");
    });

    RuleFor(r => r.Request.ManagerUserId)
      .MustAsync(async (id, _) =>
      {
        Guid creatorId = httpContextAccessor.HttpContext.GetUserId();

        List<Guid> departmentManagers = await departmentService.GetDepartmentManagersByUserId(creatorId);
        List<Guid> projectManagers = await projectService.GetProjectManagersByUserId(creatorId);

        return departmentManagers.Union(projectManagers).Any(m => m == id);
      })
      .WithMessage("There is no project or department manager with provided id.");
  }
}
