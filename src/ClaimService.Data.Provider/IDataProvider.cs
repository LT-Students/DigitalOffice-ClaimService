using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;

namespace ClaimService.Data.Provider;

[AutoInject(InjectType.Scoped)]
public interface IDataProvider : IBaseDataProvider
{
}
