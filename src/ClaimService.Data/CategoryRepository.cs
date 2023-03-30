using System;
using System.Linq;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Provider;

namespace LT.DigitalOffice.ClaimService.Data;

public class CategoryRepository : ICategoryRepository
{
  private readonly IDataProvider _provider;

  public CategoryRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public bool DoesExistAsync(Guid categoryId)
  {
    return _provider.Categories.Any(category => category.Id == categoryId && category.IsActive);
  }
}
