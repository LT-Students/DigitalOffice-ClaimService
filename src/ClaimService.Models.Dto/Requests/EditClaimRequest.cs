﻿using System;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Requests;

public record EditClaimRequest
{
  public string Name { get; set; }
  public string Content { get; set; }
  public Guid? CategoryId { get; set; }
  public ClaimUrgency? Urgency { get; set; }
  public ClaimStatus? Status { get; set; }
  public DateTime? Deadline { get; set; }
}