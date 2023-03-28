using System;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Data.Provider;
using LT.DigitalOffice.ClaimService.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.Data;

public class CategoryRepository : ICategoryRepository
{
  private readonly IDataProvider _provider;

  public CategoryRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<bool> DoesExistAsync(Guid categoryId)
  {
    return await _provider.Categories.AnyAsync(category => category.Id == categoryId && category.IsActive);
  }
}
