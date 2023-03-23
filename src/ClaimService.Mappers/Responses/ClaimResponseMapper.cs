using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Responses;

namespace LT.DigitalOffice.ClaimService.Mappers.Responses;

public class ClaimResponseMapper : IClaimResponseMapper
{
  public ClaimResponse Map (DbClaim dbClaim, UserInfo userInfo)
  {
    return dbClaim is null
      ? null
      : new ClaimResponse
      {
        Id = dbClaim.Id,
        Name = dbClaim.Name,
        Content = dbClaim.Content,
        Status = dbClaim.Status,
        Urgency = dbClaim.Urgency,
        DeadLine = dbClaim.DeadLine,
        CreatedBy = userInfo,
        CreatedAtUtc = dbClaim.CreatedAtUtc,
      };
  }
}
