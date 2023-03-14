using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbClaimCategoryExecutor
{
  public const string TableName = "ClaimCategoryExecutors";

  public Guid Id { get; set; }
  public Guid ExecutorId { get; set; }
  public Guid CategoryId { get; set; }
  public Guid ExecutorManagerId { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbClaimCategory Category { get; set; }
}

public class DbClaimCategoryExecutorConfiguration : IEntityTypeConfiguration<DbClaimCategoryExecutor>
{
  public void Configure(EntityTypeBuilder<DbClaimCategoryExecutor> builder)
  {
    builder
      .ToTable(DbClaimCategoryExecutor.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(cce => cce.Category)
      .WithMany(c => c.Executors);
  }
}
