using System;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.ClaimService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  bool DoesExistAsync(Guid categoryId);
}
