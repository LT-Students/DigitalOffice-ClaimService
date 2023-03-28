﻿using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.Data.Provider.MsSql.Ef;

public class ClaimServiceDbContext : DbContext, IDataProvider
{
  public DbSet<DbClaim> Claims { get; set; }
  public DbSet<DbChangeArgument> ChangeArguments { get; set; }
  public DbSet<DbChangeTemplate> ChangeTemplates { get; set; }
  public DbSet<DbCategory> Categories { get; set; }
  public DbSet<DbCategoryExecutor> CategoryExecutors { get; set; }
  public DbSet<DbClaimChange> ClaimChanges { get; set; }
  public DbSet<DbClaimFile> ClaimFiles { get; set; }
  public DbSet<DbClaimImage> ClaimImages { get; set; }
  public DbSet<DbClaimComment> ClaimComments { get; set; }

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
