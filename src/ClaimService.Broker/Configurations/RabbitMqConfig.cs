using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Department;
using LT.DigitalOffice.Models.Broker.Requests.Project;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.ClaimService.Models.Dto.Configurations;

public class RabbitMqConfig : BaseRabbitMqConfig
{
  [AutoInjectRequest(typeof(IGetUsersDataRequest))]
  public string GetUsersDataEndpoint { get; set; }

  [AutoInjectRequest(typeof(IGetDepartmentsRequest))]
  public string GetDepartmentsEndpoint { get; set; }

  [AutoInjectRequest(typeof(IGetProjectsRequest))]
  public string GetProjectsEndpoint { get; set; }
}
