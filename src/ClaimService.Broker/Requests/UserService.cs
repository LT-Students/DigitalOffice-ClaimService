﻿using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Broker.Requests;

public class UserService : IUserService
{
  private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsersData;

  public UserService(IRequestClient<IGetUsersDataRequest> rcGetUsersData)
  {
    _rcGetUsersData = rcGetUsersData;
  }

  public async Task<bool> DoesUserExist(Guid userId)
  {
    if (userId == default)
    {
      return false;
    }

    return (await _rcGetUsersData.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(
      IGetUsersDataRequest.CreateObj(new List<Guid> { userId }))).UsersData
      .Any(u => u.Id == userId);
  }
}
