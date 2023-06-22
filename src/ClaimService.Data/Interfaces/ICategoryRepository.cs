using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  Task<bool> DoesExistAsync(Guid categoryId);
}
