using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Requests.Project;
using LT.DigitalOffice.Models.Broker.Responses.Project;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Broker.Requests;
public class ProjectService : IProjectService
{
  private readonly IRequestClient<IGetProjectsRequest> _rcGetProjectUsers;

  public ProjectService(IRequestClient<IGetProjectsRequest> rcGetProjectUsers)
  {
    _rcGetProjectUsers = rcGetProjectUsers;
  }

  public async Task<List<Guid>> GetProjectManagersByUserId(Guid userId)
  {
    if (userId == default)
    {
      return new List<Guid>();
    }

    return (await _rcGetProjectUsers.ProcessRequest<IGetProjectsRequest, IGetProjectsResponse>(
      IGetProjectsUsersRequest.CreateObj(usersIds: new List<Guid> { userId }, isActive: true))).Projects
      .SelectMany(p => p.Users)
      .Where(u => u.ProjectUserRole == ProjectUserRoleType.Manager)
      .Select(u => u.UserId)
      .Distinct()
      .ToList();
  }
}
