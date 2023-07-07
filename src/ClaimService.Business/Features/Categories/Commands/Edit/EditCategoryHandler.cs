using FluentValidation.Results;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;

public class EditCategoryHandler : IRequestHandler<EditCategoryCommand, Unit>
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IAccessValidator _accessValidator;
  private readonly IEditCategoryValidator _validator;

  public EditCategoryHandler(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor,
    IAccessValidator accessValidator,
    IEditCategoryValidator validator)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
    _accessValidator = accessValidator;
    _validator = validator;
  }

  public async Task<Unit> Handle(EditCategoryCommand command, CancellationToken ct)
  {
    DbCategory category = await _provider.Categories.FirstOrDefaultAsync(c => c.Id == command.CategoryId && c.IsActive, ct);
    if (category is null)
    {
      throw new BadRequestException("No category with provided wid as found.");
    }

    Guid editorId = _httpContextAccessor.HttpContext.GetUserId();
    if (!await _accessValidator.IsAdminAsync(editorId))
    {
      throw new ForbiddenException("Not enough rights to edit category.");
    }

    ValidationResult validationResult = await _validator.ValidateAsync(command.Patch, ct);
    if (!validationResult.IsValid)
    {
      throw new BadRequestException(validationResult.Errors.Select(e => e.ErrorMessage));
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
