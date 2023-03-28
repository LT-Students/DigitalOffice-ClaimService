using LT.DigitalOffice.ClaimService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.ClaimService.Data.Provider.MsSql.Ef.Migrations;

[DbContext(typeof(ClaimServiceDbContext))]
[Migration("20230327173300_RemaneClaimUrgencyColuumnToPriority")]
public class RemaneClaimUrgencyColuumnToPriority : Migration
{
  public void RenameUrgencyColummnToPriority(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.RenameColumn(
      name: "Urgency",
      table: DbClaim.TableName,
      newName: "Priority");
  }

  protected override void Up(MigrationBuilder migrationBuilder)
  {
    RenameUrgencyColummnToPriority(migrationBuilder);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    base.Down(migrationBuilder);
  }
}
