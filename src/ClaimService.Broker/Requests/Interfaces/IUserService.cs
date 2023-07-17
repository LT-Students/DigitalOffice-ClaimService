using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;

[AutoInject]
public interface IUserService
{
  Task<bool> DoesUserExist(Guid userId);
}
