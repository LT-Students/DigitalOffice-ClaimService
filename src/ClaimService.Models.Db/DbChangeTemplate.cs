using System;
using System.Collections.Generic;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbChangeTemplate
{
  public const string TableName = "ChangeTemplates";

  public Guid Id { get; set; }
  public ChangeType ChangeType { get; set; }
  public Locale Locale { get; set; }
  public string Content { get; set; }

  public ICollection<DbClaimChange> ClaimChanges { get; set; }
}

public class DbChangeTemplateConfiguration : IEntityTypeConfiguration<DbChangeTemplate>
{
  public void Configure(EntityTypeBuilder<DbChangeTemplate> builder)
  {
    builder
      .ToTable(DbChangeTemplate.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasMany(ct => ct.ClaimChanges)
      .WithOne(ch => ch.Template);
  }
}
