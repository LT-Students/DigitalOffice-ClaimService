using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.ClaimService.DataLayer.Models;

public class DbCategory
{
  public const string TableName = "Categories";

  public Guid Id { get; set; }
  public string Name { get; set; }
  public int Color { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public IList<DbClaim> Claims { get; set; } = new List<DbClaim>();
}

public class DbCategoryConfiguration : IEntityTypeConfiguration<DbCategory>
{
  public void Configure(EntityTypeBuilder<DbCategory> builder)
  {
    builder.ToTable(DbCategory.TableName);

    builder.HasKey(c => c.Id);

    builder.HasMany(c => c.Claims).WithOne(c => c.Category);
  }
}
