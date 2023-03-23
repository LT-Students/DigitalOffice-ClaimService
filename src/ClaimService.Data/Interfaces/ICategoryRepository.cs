using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  Task<bool> DoesExistAsync(Guid categoryId);
}
