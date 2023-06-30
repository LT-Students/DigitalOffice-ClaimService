using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Edit;

public record EditClaimCommand : IRequest<Unit>
{
  public Guid ClaimId { get; set; }
  public JsonPatchDocument<EditClaimRequest> Patch { get; set; }
}
