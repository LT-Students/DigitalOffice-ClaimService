using System;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Mappers.Db.Intterfaces;

[AutoInject]
public interface IDbClaimMapper
{
  DbClaim Map(CreateClaimRequest request, Guid senderId);
}
