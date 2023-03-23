using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.ClaimService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.ClaimService.Mappers.Models.Interfaces;

[AutoInject]
public interface IUserInfoMapper
{
  List<UserInfo> Map(List<UserData> usersData);
}
