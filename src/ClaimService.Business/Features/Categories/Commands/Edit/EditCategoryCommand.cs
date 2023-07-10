using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;

public class EditCategoryCommand : IRequest<Unit>
{
  public Guid CategoryId { get; set; }
  public JsonPatchDocument<EditCategoryRequest> Patch { get; set; }
}
