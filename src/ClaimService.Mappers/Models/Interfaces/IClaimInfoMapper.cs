using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;

[AutoInject]
public interface IClaimInfoMapper
{
  List<ClaimInfo> Map(List<DbClaim> dbClaims);
}
