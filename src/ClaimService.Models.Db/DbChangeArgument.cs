using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbChangeArgument
{
  public const string TableName = "ChangeArguments";

  public Guid Id { get; set; }
  public Guid ChangeId { get; set; }
  public string Argument { get; set; }
  public int Position { get; set; }

  public DbClaimChange ClaimChange { get; set; }
}

public class DbChangeArgumentConfiguration : IEntityTypeConfiguration<DbChangeArgument>
{
  public void Configure(EntityTypeBuilder<DbChangeArgument> builder)
  {
    builder
      .ToTable(DbChangeArgument.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(ct => ct.ClaimChange)
      .WithMany(ch => ch.Arguments);
  }
}
