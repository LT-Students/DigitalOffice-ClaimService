using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  Task DoesExistAsync(Guid categoryId);
}
