using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;

[AutoInject]
public interface IEditCategoryValidator : IValidator<JsonPatchDocument<EditCategoryRequest>>
{
}
