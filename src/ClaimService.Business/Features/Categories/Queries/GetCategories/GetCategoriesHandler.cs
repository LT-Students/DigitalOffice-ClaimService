using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategories;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, FindResult<CategoryInfo>>
{
  private readonly IDataProvider _provider;

  public GetCategoriesHandler(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<FindResult<CategoryInfo>> Handle(GetCategoriesQuery query, CancellationToken ct)
  {
    (List<DbCategory> categories, int totalCount) = await GetAsync(query, ct);
    return new FindResult<CategoryInfo>
    {
      Body = categories.ConvertAll(Map),
      TotalCount = totalCount
    };
  }

  private CategoryInfo Map(DbCategory category)
  {
    return new()
    {
      Id = category.Id,
      Name = category.Name,
      Color = (Color)category.Color,
      IsActive = category.IsActive
    };
  }

  private async Task<(List<DbCategory> categories, int totalcount)> GetAsync(GetCategoriesQuery query, CancellationToken ct)
  {
    IQueryable<DbCategory> categories = query.IncludeDeactivated
      ? _provider.Categories.AsNoTracking()
      : _provider.Categories.AsNoTracking().Where(c => c.IsActive);

    if (!string.IsNullOrWhiteSpace(query.NameIncludeSubstring))
    {
      categories = categories.Where(c => c.Name.Contains(query.NameIncludeSubstring));
    }

    if (query.Color.HasValue)
    {
      categories = categories.Where(c => c.Color == (int)query.Color.Value);
    }

    if (query.IsAscendingSort.HasValue)
    {
      categories = query.IsAscendingSort.Value
        ? categories.OrderBy(c => c.Id)
        : categories.OrderByDescending(c => c.Id);
    }

    return (
      await categories
        .Skip(query.SkipCount)
        .Take(query.TakeCount)
        .ToListAsync(ct),
      await categories.CountAsync(ct));
  }
}
