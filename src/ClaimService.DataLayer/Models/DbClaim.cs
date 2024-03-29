﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.ClaimService.DataLayer.Models;

public class DbClaim
{
  public const string TableName = "Claims";

  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Content { get; set; }
  public Guid? CategoryId { get; set; }
  public Guid? DepartmentId { get; set; }
  public int Status { get; set; }
  public int Priority { get; set; }
  public DateTime? DeadLine { get; set; }
  public Guid? ResponsibleUserId { get; set; }
  public Guid ManagerUserId { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbCategory Category { get; set; }
  public ICollection<DbClaimComment> Comments { get; set; } = new List<DbClaimComment>();
}

public class DbClaimConfiguration : IEntityTypeConfiguration<DbClaim>
{
  public void Configure(EntityTypeBuilder<DbClaim> builder)
  {
    builder.ToTable(DbClaim.TableName);

    builder.HasKey(c => c.Id);

    builder.HasOne(c => c.Category).WithMany(c => c.Claims);

    builder.HasMany(c => c.Comments).WithOne(c => c.Claim);
  }
}
