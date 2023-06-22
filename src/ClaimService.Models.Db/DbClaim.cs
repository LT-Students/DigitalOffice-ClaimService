using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbClaim
{
  public const string TableName = "Claims";

  public Guid Id { get; set; }
  public string Name { get; set; }
  public Guid CategoryId { get; set; }
  public string Content { get; set; }
  public ClaimStatus Status { get; set; }
  public ClaimPriority Priority { get; set; }
  public DateTime? DeadLine { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbCategory Category { get; set; }
  public ICollection<DbClaimFile> Files { get; set; }
  public ICollection<DbClaimImage> Images { get; set; }
  public ICollection<DbClaimComment> Comments { get; set; }
  public ICollection<DbClaimChange> Changes { get; set; }
}

public class DbClaimConfiguration : IEntityTypeConfiguration<DbClaim>
{
  public void Configure(EntityTypeBuilder<DbClaim> builder)
  {
    builder
      .ToTable(DbClaim.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(c => c.Category)
      .WithMany(cc => cc.Claims);

    builder
      .HasMany(c => c.Files)
      .WithOne(cf => cf.Claim);

    builder
      .HasMany(c => c.Images)
      .WithOne(cf => cf.Claim);

    builder
      .HasMany(c => c.Comments)
      .WithOne(cf => cf.Claim);

    builder
      .HasMany(c => c.Changes)
      .WithOne(cf => cf.Claim);
  }
}
