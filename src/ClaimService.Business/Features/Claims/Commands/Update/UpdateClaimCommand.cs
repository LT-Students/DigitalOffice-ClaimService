using MediatR;
using System;

namespace LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Update;

public record UpdateClaimCommand : IRequest<Unit>
{
  public Guid ClaimId { get; set; }
  public UpdateClaimRequest Request { get; set; }
}
