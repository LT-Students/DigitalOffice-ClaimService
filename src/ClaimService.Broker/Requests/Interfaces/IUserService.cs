using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;

[AutoInject]
public interface IUserService
{
  Task<List<UserData>> GetUsersDataAsync(List<Guid> usersIds);
}
