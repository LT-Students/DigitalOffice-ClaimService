using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.ClaimService.Mappers.Db.Intterfaces;

[AutoInject]
public interface IDbClaimMapper
{
  DbClaim Map(CreateClaimRequest request, Guid senderId);
}
