using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbClaimImage
{
  public const string TableName = "ClaimImages";

  public Guid Id { get; set; }
  public Guid ClaimId { get; set; }
  public Guid ImageId { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }

  public DbClaim Claim { get; set; }
}

public class DbClaimImageConfiguration : IEntityTypeConfiguration<DbClaimImage>
{
  public void Configure(EntityTypeBuilder<DbClaimImage> builder)
  {
    builder
      .ToTable(DbClaimImage.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(ci => ci.Claim)
      .WithMany(c => c.Images);
  }
}
