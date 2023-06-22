using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;

namespace LT.DigitalOffice.ClaimService.Mappers.Models;

public class ClaimInfoMapper : IClaimInfoMapper
{
  public ClaimInfo Map(DbClaim dbClaim)
  {
    return new ClaimInfo
    {
      Id = dbClaim.Id,
      Name = dbClaim.Name,
      Content = dbClaim.Content,
      CategoryId = dbClaim.CategoryId,
      Status = dbClaim.Status,
      Priority = dbClaim.Priority,
      DeadLine = dbClaim.DeadLine,
      CreatedBy = dbClaim.CreatedBy,
      CreatedAtUtc = dbClaim.CreatedAtUtc,
    };
  }
}
