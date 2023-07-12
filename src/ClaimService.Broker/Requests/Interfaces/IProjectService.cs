using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;

[AutoInject]
public interface IProjectService
{
  Task<List<Guid>> GetProjectManagersByUserId(Guid userId);
}
