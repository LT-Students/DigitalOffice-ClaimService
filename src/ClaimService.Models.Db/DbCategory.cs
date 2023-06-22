using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbCategory
{
  public const string TableName = "Categories";

  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public ICollection<DbClaim> Claims { get; set; }
  public ICollection<DbCategoryExecutor> Executors { get; set; }
}

public class DbClaimCategoryConfiguration : IEntityTypeConfiguration<DbCategory>
{
  public void Configure(EntityTypeBuilder<DbCategory> builder)
  {
    builder
      .ToTable(DbCategory.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasMany(cc => cc.Executors)
      .WithOne(c => c.Category);

    builder
      .HasMany(cc => cc.Claims)
      .WithOne(c => c.Category);
  }
}
