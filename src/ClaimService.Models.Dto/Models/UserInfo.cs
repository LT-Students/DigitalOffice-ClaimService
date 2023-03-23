using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Models;

public record UserInfo
{
  public Guid UserId { get; set; }
  public string FirstName { get; set; }
  public string MiddleName { get; set; }
  public string LastName { get; set; }
  public Guid? ImageId { get; set; }
}
