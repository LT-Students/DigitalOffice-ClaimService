﻿using LT.DigitalOffice.ClaimService.Models.Dto.Requests.Claim;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Business.Commands.Claim.Interfaces;

[AutoInject]
public interface ICreateClaimCommand
{
  Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateClaimRequest request, CancellationToken cancellationToken);
}
