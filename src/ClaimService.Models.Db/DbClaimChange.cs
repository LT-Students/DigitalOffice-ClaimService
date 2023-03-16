using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbClaimChange
{
  public const string TableName = "ClaimChanges";

  public Guid Id { get; set; }
  public Guid ClaimId { get; set; }
  public DateTime ChangedAtUtc { get; set; }
  public Guid ChangeAuthorId { get; set; }
  public string ChangeAuthorName { get; set; }
  public Guid TemplateId { get; set; }

  public DbChangeTemplate Template { get; set; }
  public DbClaim Claim { get; set; }
  public ICollection<DbChangeArgument> Arguments { get; set; }
}

public class DbClaimChangeConfiguration : IEntityTypeConfiguration<DbClaimChange>
{
  public void Configure(EntityTypeBuilder<DbClaimChange> builder)
  {
    builder
      .ToTable(DbClaimChange.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(ch => ch.Template)
      .WithMany(ct => ct.ClaimChanges);

    builder
      .HasOne(ch => ch.Claim)
      .WithMany(c => c.Changes);

    builder
      .HasMany(ch => ch.Arguments)
      .WithOne(ca => ca.ClaimChange);
  }
}
