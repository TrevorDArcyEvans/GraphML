using RestSharp;

namespace GraphML.API.Server
{
  public interface IRestClientFactory
  {
    IRestClient GetRestClient();
  }
}
