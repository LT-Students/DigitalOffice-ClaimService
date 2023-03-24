using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.ClaimService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Mappers.Responses.Interfaces;

[AutoInject]
public interface IClaimResponseMapper
{
  ClaimResponse Map(DbClaim dbClaim, UserInfo userInfo);
}
