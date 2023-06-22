using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;

[AutoInject]
public interface IUserService
{
  Task<List<UserData>> GetUsersDataAsync(List<Guid> usersIds);
}
