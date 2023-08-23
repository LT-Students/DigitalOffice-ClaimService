using LT.DigitalOffice.ClaimService.DataLayer.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ClaimService.DataLayer;

[AutoInject(InjectType.Scoped)]
public interface IDataProvider : IBaseDataProvider
{
  public DbSet<DbClaim> Claims { get; set; }
  public DbSet<DbCategory> Categories { get; set; }
  public DbSet<DbClaimComment> Comments { get; set; }
}
