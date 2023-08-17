using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Update;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Unit>
{
  private readonly IDataProvider _provider;
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UpdateCategoryHandler(
    IDataProvider provider,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<Unit> Handle(UpdateCategoryCommand command, CancellationToken ct)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();
    // TODO: add right check.
    if (!await _accessValidator.IsAdminAsync(senderId))
    {
      throw new ForbiddenException("Not enough rights to edit claim category.");
    }

    DbCategory category = await _provider.Categories.FirstAsync(c => c.Id == command.CategoryId, ct);
    UpdateCategoryRequest request = command.Request;

    category.Name = request.Name;
    category.Color = (int)request.Color;
    category.ModifiedBy = senderId;
    category.ModifiedAtUtc = DateTime.UtcNow;

    await _provider.SaveAsync();

    return Unit.Value;
  }
}
