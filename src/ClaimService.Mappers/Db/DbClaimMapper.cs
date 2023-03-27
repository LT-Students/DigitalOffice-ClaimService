using System;
using LT.DigitalOffice.ClaimService.Mappers.Db.Intterfaces;
using LT.DigitalOffice.ClaimService.Models.Db;
using LT.DigitalOffice.ClaimService.Models.Dto.Enums;
using LT.DigitalOffice.ClaimService.Models.Dto.Requests;

namespace LT.DigitalOffice.ClaimService.Mappers.Db;

public class DbClaimMapper : IDbClaimMapper
{
  public DbClaim Map(CreateClaimRequest request, Guid senderId)
  {
    return request is null
      ? null
      : new DbClaim
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        CategoryId = request.CategoryId,
        Content = request.Content,
        Status = ClaimStatus.Created,
        Priority = request.Priority.HasValue ? request.Priority.Value : ClaimPriority.Magor,
        DeadLine = request.Deadline,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = senderId
      };
  }
}
