using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbCategoryExecutor
{
  public const string TableName = "CategoryExecutors";

  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid CategoryId { get; set; }
  public Guid ExecutorManagerId { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbCategory Category { get; set; }
}

public class DbClaimCategoryExecutorConfiguration : IEntityTypeConfiguration<DbCategoryExecutor>
{
  public void Configure(EntityTypeBuilder<DbCategoryExecutor> builder)
  {
    builder
      .ToTable(DbCategoryExecutor.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(cce => cce.Category)
      .WithMany(c => c.Executors);
  }
}
