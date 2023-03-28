using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;

[AutoInject]
public interface IUserInfoMapper
{
  UserInfo Map(UserData userData);
}
