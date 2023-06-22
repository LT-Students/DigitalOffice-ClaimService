using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbClaimFile
{
  public const string TableName = "ClaimFiles";

  public Guid Id { get; set; }
  public Guid ClaimId { get; set; }
  public Guid FileId { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }

  public DbClaim Claim { get; set; }
}

public class DbClaimFileConfiguration : IEntityTypeConfiguration<DbClaimFile>
{
  public void Configure(EntityTypeBuilder<DbClaimFile> builder)
  {
    builder
      .ToTable(DbClaimFile.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(cf => cf.Claim)
      .WithMany(c => c.Files);
  }
}
