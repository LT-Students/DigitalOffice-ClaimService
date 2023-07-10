using LT.DigitalOffice.ClaimService.Business.Shared.Enums;
using LT.DigitalOffice.ClaimService.DataLayer;
using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;

public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryResponse>
{
  private readonly IDataProvider _provider;

  public GetCategoryHandler(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<CategoryResponse> Handle(GetCategoryQuery query, CancellationToken ct)
  {
    DbCategory category = await GetAsync(query, ct);
    if (category is null)
    {
      throw new NotFoundException("No category was found.");
    }

    return Map(category);
  }

  private Task<DbCategory> GetAsync(GetCategoryQuery query, CancellationToken ct)
  {
    return _provider.Categories.AsNoTracking()
      .Where(c => c.Id == query.CategoryId)
      .FirstOrDefaultAsync(ct);
  }

  private CategoryResponse Map(DbCategory category)
  {
    return new()
    {
      Id = category.Id,
      Name = category.Name,
      Color = (Color)category.Color,
      IsActive = category.IsActive
    };
  }
}
