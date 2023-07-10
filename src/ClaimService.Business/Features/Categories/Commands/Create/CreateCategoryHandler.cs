using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public CreateCategoryHandler(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
  }

  public Task<Guid> Handle(CreateCategoryCommand command, CancellationToken ct)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();

    return CreateAsync(Map(command, senderId));
  }

  private async Task<Guid> CreateAsync(DbCategory category)
  {
    _provider.Categories.Add(category);
    await _provider.SaveAsync();

    return category.Id;
  }

  private DbCategory Map(CreateCategoryCommand command, Guid senderId)
  {
    return new()
    {
      Id = Guid.NewGuid(),
      Name = command.Name,
      Color = (int)command.Color,
      IsActive = true,
      CreatedAtUtc = DateTime.UtcNow,
      CreatedBy = senderId
    };
  }
}
