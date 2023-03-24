using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;

namespace LT.DigitalOffice.ClaimService.Broker.Requests;

public class UserService : IUserService
{
  private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsersData;

  public UserService(IRequestClient<IGetUsersDataRequest> rcGetUsersData)
  {
    _rcGetUsersData = rcGetUsersData;
  }

  public async Task<List<UserData>> GetUsersDataAsync(List<Guid> usersIds)
  {
    if (usersIds is null || !usersIds.Any())
    {
      return null;
    }

    return (await _rcGetUsersData.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(
      IGetUsersDataRequest.CreateObj(usersIds)))
      ?.UsersData;
  }
}
