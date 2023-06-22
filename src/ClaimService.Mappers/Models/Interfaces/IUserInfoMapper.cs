using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;

[AutoInject]
public interface IUserInfoMapper
{
  UserInfo Map(UserData userData);
}
