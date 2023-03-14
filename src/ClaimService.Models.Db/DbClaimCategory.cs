using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbClaimCategory
{
  public const string TableName = "ClaimCategories";
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbClaim Claim { get; set; }
  public ICollection<DbClaimCategoryExecutor> Executors { get; set; }
}

public class DbClaimCategoryConfiguration : IEntityTypeConfiguration<DbClaimCategory>
{
  public void Configure(EntityTypeBuilder<DbClaimCategory> builder)
  {
    builder
      .ToTable(DbClaimCategory.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasMany(cc => cc.Executors)
      .WithOne(c => c.Category);

    builder
      .HasOne(cc => cc.Claim)
      .WithOne(c => c.Category);
  }
}
