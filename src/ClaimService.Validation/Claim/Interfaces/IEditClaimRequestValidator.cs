using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.ClaimService.Validation.Claim.Interfaces;

[AutoInject]
public interface IEditClaimRequestValidator : IValidator<(Guid, JsonPatchDocument<EditClaimRequest>)>
{
}
