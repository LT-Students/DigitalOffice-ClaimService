using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.ClaimService.Data.Interfaces;

[AutoInject]
public interface IClaimRepository
{
  Task<Guid?> CreateAsync(DbClaim claim);
  Task<(List<DbClaim> dbClaim, int totalcount)> FindAsync(FindClaimFilter filter, CancellationToken cancellationToken = default);
  Task<DbClaim> GetAsync(GetClaimFilter filter, CancellationToken cancellationToken = default);
  Task<DbClaim> EditAsync(Guid claimId, JsonPatchDocument<DbClaim> patch, Guid modifierId, CancellationToken cancellationToken = default);
}
