using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClaimService.Data.Provider.MsSql.Ef;

public class ClaimServiceDbContext : DbContext, IDataProvider
{
  public ClaimServiceDbContext(DbContextOptions<ClaimServiceDbContext> options)
      : base(options)
  {
  }

  public void Save()
  {
    SaveChanges();
  }

  public async Task SaveAsync()
  {
    await SaveChangesAsync();
  }

  public object MakeEntityDetached(object obj)
  {
    Entry(obj).State = EntityState.Detached;
    return Entry(obj).State;
  }

  public void EnsureDeleted()
  {
    Database.EnsureDeleted();
  }

  public bool IsInMemory()
  {
    return Database.IsInMemory();
  }
}
