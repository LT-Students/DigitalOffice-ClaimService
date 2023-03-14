using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.ClaimService.Models.Db;

public class DbComment
{
  public const string TableName = "Comments";

  public Guid Id { get; set; }
  public string Content { get; set; }
  public Guid ClaimId { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbClaim Claim { get; set; }
}

public class DbCommentConfiguration : IEntityTypeConfiguration<DbComment>
{
  public void Configure(EntityTypeBuilder<DbComment> builder)
  {
    builder
      .ToTable(DbComment.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(com => com.Claim)
      .WithMany(c => c.Comments);
  }
}
