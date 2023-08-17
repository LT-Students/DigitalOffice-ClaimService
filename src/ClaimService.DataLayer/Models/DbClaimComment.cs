using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.ClaimService.DataLayer.Models;

public class DbClaimComment
{
  public const string TableName = "ClaimComments";

  public Guid Id { get; set; }
  public Guid ClaimId { get; set; }
  public string Content { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbClaim Claim { get; set; }
}

public class DbEventCommentsConfiguration : IEntityTypeConfiguration<DbClaimComment>
{
  public void Configure(EntityTypeBuilder<DbClaimComment> builder)
  {
    builder.ToTable(DbClaimComment.TableName);

    builder.HasKey(t => t.Id);

    builder.HasOne(e => e.Claim).WithMany(ec => ec.Comments);
  }
}
