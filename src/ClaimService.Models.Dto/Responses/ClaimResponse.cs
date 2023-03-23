using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using Microsoft.AspNetCore.Identity;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Responses;

public class ClaimResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Content { get; set; }
  public ClaimStatus Status { get; set; }
  public ClaimUrgency Urgency { get; set; }
  public DateTime? DeadLine { get; set; }
  public UserInfo CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
}
