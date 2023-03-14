using System;
using ClaimService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.ClaimService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.ClaimService.Data.Provider.MsSql.Ef.Migrations;

[DbContext(typeof(ClaimServiceDbContext))]
[Migration("20230314145500_InitialCreate")]
public class InitialCreate : Migration
{
  private void CreateChangeArgumentsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbChangeArgument.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        ChangeId = table.Column<Guid>(nullable: false),
        Argument = table.Column<string>(nullable: false),
        Position = table.Column<int>(nullable: false)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbChangeArgument.TableName}", ca => ca.Id);
      });
  }

  private void CreateChangeTemplatesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbChangeTemplate.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        ChangeType = table.Column<int>(nullable: false),
        Locale = table.Column<int>(nullable: false),
        Content = table.Column<string>(nullable: true),
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbChangeTemplate.TableName}", ct => ct.Id);
      });
  }

  private void CreateClaimsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbClaim.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Name = table.Column<string>(nullable: false),
        CategoryId = table.Column<Guid>(nullable: false),
        Content = table.Column<string>(nullable: false),
        Status = table.Column<int>(nullable: false),
        Urgensy = table.Column<int>(nullable: false),
        DeadLine = table.Column<DateTime>(nullable: true),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbClaim.TableName}", c => c.Id);
      });
  }

  private void CreateClaimCategoriesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbClaimCategory.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Name = table.Column<string>(nullable: false),
        Description = table.Column<string>(nullable: true),
        IsActive = table.Column<bool>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbClaimCategory.TableName}", cc => cc.Id);
      });
  }

  private void CreateClaimCategoryExecutorsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbClaimCategoryExecutor.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        ExecutorId = table.Column<Guid>(nullable: false),
        CategoryId = table.Column<Guid>(nullable: false),
        ExecutorManagerId = table.Column<Guid>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbClaimCategoryExecutor.TableName}", cce => cce.Id);
      });
  }

  private void CreateClaimChangesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbClaimChange.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        ClaimId = table.Column<Guid>(nullable: false),
        ChangedAtUtc = table.Column<DateTime>(nullable: false),
        ChangeAuthorId = table.Column<Guid>(nullable: false),
        ChangeAuthorName = table.Column<string>(nullable: false),
        TemplateId = table.Column<Guid>(nullable: false)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbClaimChange.TableName}", ch => ch.Id);
      });
  }

  private void CreateClaimFilesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbClaimFile.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        ClaimId = table.Column<Guid>(nullable: false),
        FileId = table.Column<Guid>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbClaimFile.TableName}", cf => cf.Id);
      });
  }

  private void CreateClaimImagesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbClaimImage.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        ClaimId = table.Column<Guid>(nullable: false),
        ImageId = table.Column<Guid>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbClaimImage.TableName}", ci => ci.Id);
      });
  }

  private void CreateCommentsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbComment.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Content = table.Column<string>(nullable: false),
        ClaimId = table.Column<Guid>(nullable: false),
        IsActive = table.Column<bool>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbComment.TableName}", com => com.Id);
      });
  }

  protected override void Up(MigrationBuilder migrationBuilder)
  {
    CreateChangeArgumentsTable(migrationBuilder);
    CreateChangeTemplatesTable(migrationBuilder);
    CreateClaimsTable(migrationBuilder);
    CreateCommentsTable(migrationBuilder);
    CreateClaimImagesTable(migrationBuilder);
    CreateClaimFilesTable(migrationBuilder);
    CreateClaimChangesTable(migrationBuilder);
    CreateClaimCategoryExecutorsTable(migrationBuilder);
    CreateClaimCategoriesTable(migrationBuilder);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(DbComment.TableName);
    migrationBuilder.DropTable(DbClaimImage.TableName);
    migrationBuilder.DropTable(DbClaimFile.TableName);
    migrationBuilder.DropTable(DbClaimChange.TableName);
    migrationBuilder.DropTable(DbClaimCategoryExecutor.TableName);
    migrationBuilder.DropTable(DbClaimCategory.TableName);
    migrationBuilder.DropTable(DbClaim.TableName);
    migrationBuilder.DropTable(DbChangeTemplate.TableName);
    migrationBuilder.DropTable(DbChangeArgument.TableName);
  }
}
