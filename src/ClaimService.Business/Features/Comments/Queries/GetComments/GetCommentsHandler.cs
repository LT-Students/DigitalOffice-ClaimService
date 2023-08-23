using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Comments.Queries.GetComments;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, FindResult<CommentInfo>>
{
  private readonly IDataProvider _provider;

  public GetCommentsHandler(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<FindResult<CommentInfo>> Handle(GetCommentsQuery query, CancellationToken ct)
  {
    if (!await _provider.Claims.AnyAsync(c => c.Id == query.ClaimId, ct))
    {
      throw new NotFoundException("No claim with provided id was found.");
    }

    (List<DbClaimComment> comments, int totalCount) = await GetAsync(query, ct);

    return new()
    {
      Body = comments.ConvertAll(Map),
      TotalCount = totalCount
    };
  }

  private async Task<(List<DbClaimComment>, int totalCount)> GetAsync(GetCommentsQuery query, CancellationToken ct)
  {
    IQueryable<DbClaimComment> comments = _provider.Comments.AsNoTracking().Where(c => c.IsActive && c.ClaimId == query.ClaimId);

    GetCommentsParameters parameters = query.Parameters;
    if (query.Parameters.IsAscendingSort.HasValue)
    {
      comments = parameters.IsAscendingSort.Value
        ? comments.OrderBy(c => c.CreatedAtUtc)
        : comments = comments.OrderByDescending(c => c.CreatedAtUtc);
    }

    return (
      await comments
        .Skip(parameters.SkipCount)
        .Take(parameters.TakeCount)
        .ToListAsync(ct),
      await comments.CountAsync(ct));
  }

  private CommentInfo Map(DbClaimComment comment)
  {
    return new()
    {
      Id = comment.Id,
      ClaimId = comment.ClaimId,
      Content = comment.Content,
      CreatedAtUtc = comment.CreatedAtUtc,
      CreatedBy = comment.CreatedBy,
      ModifiedAtUtc = comment.ModifiedAtUtc,
      ModifiedBy = comment.ModifiedBy
    };
  }
}
