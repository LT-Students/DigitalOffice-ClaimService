using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.ClaimService.Mappers.Patch;

public class PatchDbClaimMapper : IPatchDbClaimMapper
{
  public JsonPatchDocument<DbClaim> Map (JsonPatchDocument<EditClaimRequest> request)
  {
    if (request is null)
    {
      return null;
    }

    JsonPatchDocument<DbClaim> dbClaim = new ();

    foreach (Operation<EditClaimRequest> item in request.Operations)
    {
      dbClaim.Operations.Add(new Operation<DbClaim>(item.op, item.path, item.from, item.value));
    }

    return dbClaim;
  }
}
