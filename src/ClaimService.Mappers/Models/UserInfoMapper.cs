using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.ClaimService.Mappers.Models;

public class UserInfoMapper : IUserInfoMapper
{
  public UserInfo Map(UserData userData)
  {
    return new UserInfo
    {
      UserId = userData.Id,
      FirstName = userData.FirstName,
      LastName = userData.LastName,
      MiddleName = userData.MiddleName,
      ImageId = userData.ImageId
    };
  }
}
