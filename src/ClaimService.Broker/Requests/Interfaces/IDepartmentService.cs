using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;

[AutoInject]
public interface IDepartmentService
{
  Task<bool> DoesDepartmentExist(List<Guid> departmentIds);
}
