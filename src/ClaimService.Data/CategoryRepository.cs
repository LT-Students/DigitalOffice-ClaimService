using System;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using LT.DigitalOffice.ClaimService.Data.Provider;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.Data;

public class CategoryRepository : ICategoryRepository
{
  private readonly IDataProvider _provider;

  public CategoryRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public Task<bool> DoesExistAsync(Guid categoryId)
  {
    return _provider.Categories.AnyAsync(category => category.Id == categoryId && category.IsActive);
  }
}
