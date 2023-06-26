using LT.DigitalOffice.ClaimService.DataLayer.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.ClaimService.DataLayer.Migrations;

[DbContext(typeof(ClaimServiceDbContext))]
[Migration("20230321181400_CreateClaim")]
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
        Content = table.Column<string>(nullable: false),
        Status = table.Column<int>(nullable: false),
        Priority = table.Column<int>(nullable: false),
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

  protected override void Up(MigrationBuilder migrationBuilder)
  {
    CreateClaimTable(migrationBuilder);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(DbClaim.TableName);
  }
}
