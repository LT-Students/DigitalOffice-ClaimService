using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Commands.Create;

public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, Guid?>
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public CreateCommentHandler(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
  }

  public Task<Guid?> Handle(CreateCommentCommand command, CancellationToken ct)
  {
    Guid creatorId = _httpContextAccessor.HttpContext.GetUserId();

    return CreateAsync(Map(command, creatorId));
  }

  private async Task<Guid?> CreateAsync(DbClaimComment comment)
  {
    _provider.Comments.Add(comment);

    await _provider.SaveAsync();

    return comment.Id;
  }

  private DbClaimComment Map(CreateCommentCommand command, Guid creatorId)
  {
    return new()
    {
      Id = Guid.NewGuid(),
      ClaimId = command.ClaimId,
      Content = command.Request.Content,
      IsActive = true,
      CreatedBy = creatorId,
      CreatedAtUtc = DateTime.UtcNow
    };
  }
}
