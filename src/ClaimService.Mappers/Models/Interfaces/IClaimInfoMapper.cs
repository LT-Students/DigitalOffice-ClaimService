using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;

[AutoInject]
public interface IClaimInfoMapper
{
  ClaimInfo Map(DbClaim dbClaim);
}
