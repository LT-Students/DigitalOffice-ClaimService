﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.ClaimService.DataLayer.Models;

public class DbClaim
{
  public const string TableName = "Claims";

  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Content { get; set; }
  public int Status { get; set; }
  public int Priority { get; set; }
  public DateTime? DeadLine { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }
}

public class DbClaimConfiguration : IEntityTypeConfiguration<DbClaim>
{
  public void Configure(EntityTypeBuilder<DbClaim> builder)
  {
    builder.ToTable(DbClaim.TableName);

    builder.HasKey(t => t.Id);
  }
}