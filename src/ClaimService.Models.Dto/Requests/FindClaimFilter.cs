using System;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Requests;

public record FindClaimFilter : BaseFindFilter
{
  [FromQuery(Name = "isAscendingSort")]
  public bool? isAscendingSort { get; set; }

  [FromQuery(Name = "searchSubString")]
  public string searchSubString { get; set; }

  [FromQuery(Name = "categoryId")]
  public Guid? CategoryId { get; set; }

  [FromQuery(Name = "urgency")]
  public ClaimUrgency? Urgency { get; set; }

  [FromQuery(Name = "status")]
  public ClaimStatus? Status { get; set; }

  [FromQuery(Name = "deadLine")]
  public DateTime? DeadLine { get; set; }

  [FromQuery(Name = "authorId")]
  public Guid? AuthorId { get; set; }

  [FromQuery(Name = "executorId")]
  public Guid? ExecutorId { get; set; }

  [FromQuery(Name = "managerId")]
  public Guid? ManagerId { get; set; }
}
