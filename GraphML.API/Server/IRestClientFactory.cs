using RestSharp;

namespace GraphML.UI.Desktop
{
  public interface IRestClientFactory
  {
    IRestClient GetRestClient();
  }
}
