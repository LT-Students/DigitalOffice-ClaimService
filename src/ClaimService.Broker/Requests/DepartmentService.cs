using LT.DigitalOffice.ClaimService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Requests.Department;
using LT.DigitalOffice.Models.Broker.Responses.Department;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Broker.Requests;

public class DepartmentService : IDepartmentService
{
  private readonly IRequestClient<IGetDepartmentsRequest> _rcGetDepartments;

  public DepartmentService(IRequestClient<IGetDepartmentsRequest> rcGetDepartments)
  {
    _rcGetDepartments = rcGetDepartments;
  }

  public async Task<bool> DoesDepartmentExist(List<Guid> departmentIds)
  {
    if (departmentIds is null || !departmentIds.Any())
    {
      return false;
    }

    return (await _rcGetDepartments.ProcessRequest<IGetDepartmentsRequest, IGetDepartmentsResponse>(
      IGetDepartmentsRequest.CreateObj(departmentIds))).Departments.Any();
  }
}
