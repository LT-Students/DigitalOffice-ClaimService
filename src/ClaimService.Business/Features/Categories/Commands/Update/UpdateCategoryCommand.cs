using MediatR;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Update;

public record UpdateCategoryCommand : IRequest<Unit>
{
  public Guid CategoryId { get; set; }
  public UpdateCategoryRequest Request { get; set; }
}
