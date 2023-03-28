using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.ClaimService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbClaimMapper
{
  JsonPatchDocument<DbClaim> Map(JsonPatchDocument<EditClaimRequest> request);
}
