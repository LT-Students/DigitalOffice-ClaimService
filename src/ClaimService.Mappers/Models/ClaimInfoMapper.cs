using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;

namespace LT.DigitalOffice.ClaimService.Mappers.Models;

public class ClaimInfoMapper : IClaimInfoMapper
{
  public List<ClaimInfo> Map(List<DbClaim> dbClaims)
  {
    return dbClaims?.ConvertAll(c => new ClaimInfo
    {
      Id = c.Id,
      Name = c.Name,
      Content = c.Content,
      CategoryId = c.CategoryId,
      Status = c.Status,
      Priority = c.Priority,
      DeadLine = c.DeadLine ?? null,
      CreatedBy = c.CreatedBy,
      CreatedAtUtc = c.CreatedAtUtc,
    }).ToList();
  }
}
