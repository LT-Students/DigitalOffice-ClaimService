using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClaimService.Data.Provider;

[AutoInject(InjectType.Scoped)]
public interface IDataProvider : IBaseDataProvider
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
}
