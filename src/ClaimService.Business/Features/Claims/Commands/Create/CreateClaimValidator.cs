using FluentValidation;
using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.ClaimService.Validation.Claim;

public class CreateClaimValidator : AbstractValidator<CreateClaimCommand>
{
  public CreateClaimValidator(
    IDataProvider provider,
    IDepartmentService departmentService,
    IProjectService projectService,
    IUserService userService,
    IHttpContextAccessor httpContextAccessor)
  {
    RuleFor(r => r.Priority)
      .IsInEnum()
      .WithMessage("No such Priority.");

    When(request => request.Deadline.HasValue, () =>
    {
      RuleFor(r => r.Deadline)
        .Must(d => d > DateTime.UtcNow)
        .WithMessage("DeadLine must not be earlier than now.");
    });

    When(r => r.CategoryId.HasValue, () =>
    {
      RuleFor(r => r.CategoryId)
        .MustAsync((id, ct) => provider.Categories.AnyAsync(c => c.Id == id && c.IsActive, ct))
        .WithMessage("No category with provided id was found.");
    });

    When(r => r.DepartmentId.HasValue, () =>
    {
      RuleFor(r => r.DepartmentId)
        .MustAsync((id, ct) => departmentService.DoesDepartmentExist(new List<Guid> { id.Value }))
        .WithMessage("No department with provided id was found.");
    });

    RuleFor(r => r.ManagerUserId)
      .MustAsync(async (id, _) =>
      {
        Guid creatorId = httpContextAccessor.HttpContext.GetUserId();

        List<Guid> departmentManagers = await departmentService.GetDepartmentManagersByUserId(creatorId);
        List<Guid> projectManagers = await projectService.GetProjectManagersByUserId(creatorId);

        return departmentManagers.Union(projectManagers).Any(m => m == id);
      })
      .WithMessage("There is no project or department manager with provided id.");

    When(r => r.ResponsibleUserId.HasValue, () =>
    {
      RuleFor(r => r.ResponsibleUserId)
      .MustAsync((id, _) => userService.DoesUserExist(id.Value))
      .WithMessage("User with provided id doesn't exist.");
    });
  }
}
