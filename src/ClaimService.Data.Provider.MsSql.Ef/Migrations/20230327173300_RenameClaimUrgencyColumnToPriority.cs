using LT.DigitalOffice.ClaimService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.ClaimService.Data.Provider.MsSql.Ef.Migrations;

[DbContext(typeof(ClaimServiceDbContext))]
[Migration("20230327173300_RenameClaimUrgencyColumnToPriority")]
public class RenameClaimUrgencyColumnToPriority : Migration
{
  public void RenameUrgencyColumnToPriority(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.RenameColumn(
      name: "Urgency",
      table: DbClaim.TableName,
      newName: "Priority");
  }

  protected override void Up(MigrationBuilder migrationBuilder)
  {
    RenameUrgencyColumnToPriority(migrationBuilder);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    base.Down(migrationBuilder);
  }
}
