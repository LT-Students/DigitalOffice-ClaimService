using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;

public class EditCategoryHandler : IRequestHandler<EditCategoryCommand, Unit>
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IAccessValidator _accessValidator;

  public EditCategoryHandler(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor,
    IAccessValidator accessValidator)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
    _accessValidator = accessValidator;
  }

  public async Task<Unit> Handle(EditCategoryCommand command, CancellationToken ct)
  {
    DbCategory category = await _provider.Categories.FirstOrDefaultAsync(c => c.Id == command.CategoryId && c.IsActive, ct);
    if (category is null)
    {
      throw new BadRequestException();
    }

    Guid editorId = _httpContextAccessor.HttpContext.GetUserId();
    if (!await _accessValidator.IsAdminAsync(editorId))
    {
      throw new ForbiddenException();
    }

    Map(command.Patch).ApplyTo(category);
    category.ModifiedBy = editorId;
    category.ModifiedAtUtc = DateTime.UtcNow;
    await _provider.SaveAsync();

    return Unit.Value;
  }

  private JsonPatchDocument<DbCategory> Map(JsonPatchDocument<EditCategoryRequest> patch)
  {
    JsonPatchDocument<DbCategory> categoryPatch = new();

    foreach (Operation<EditCategoryRequest> item in patch.Operations)
    {
      categoryPatch.Operations.Add(new Operation<DbCategory>(item.op, item.path, item.from, item.value.ToString().Trim()));
    }

    return categoryPatch;
  }
}
