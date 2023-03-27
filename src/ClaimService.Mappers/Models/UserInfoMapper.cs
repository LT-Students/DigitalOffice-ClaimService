using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.ClaimService.Mappers.Models;

public class UserInfoMapper : IUserInfoMapper
{
  public List<UserInfo> Map(List<UserData> usersData)
  {
    return usersData?.ConvertAll(u => new UserInfo
    {
      UserId = u.Id,
      FirstName = u.FirstName,
      LastName = u.LastName,
      MiddleName = u.MiddleName,
      ImageId = u.ImageId
    }).ToList();
  }
}
