using System;
using ClaimService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.ClaimService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.ClaimService.Data.Provider.MsSql.Ef.Migrations;

[DbContext(typeof(ClaimServiceDbContext))]
[Migration("20232103181400_CreateClaimAndCategoryTables")]
public class CreateClaimAndCategoryTables : Migration
{
  public void CreateClaimTable(MigrationBuilder migrationBuilder)
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
        Urgency = table.Column<int>(nullable: false),
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

  public void CreateCategoryTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbCategory.TableName,
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
        table.PrimaryKey($"PK_{DbCategory.TableName}", c => c.Id);
      });
  }

  protected override void Up(MigrationBuilder migrationBuilder)
  {
    CreateClaimTable(migrationBuilder);
    CreateCategoryTable(migrationBuilder);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(DbClaim.TableName);
    migrationBuilder.DropTable(DbCategory.TableName);
  }
}
