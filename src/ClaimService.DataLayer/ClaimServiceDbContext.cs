using LT.DigitalOffice.ClaimService.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.DataLayer;

public class ClaimServiceDbContext : DbContext, IDataProvider
{
  public DbSet<DbClaim> Claims { get; set; }
  public DbSet<DbCategory> Categories { get; set; }

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
