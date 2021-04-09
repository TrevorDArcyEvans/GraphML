using GraphML.Common;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace GraphML.API.Server
{
  public sealed class RestClientFactory : IRestClientFactory
  {
    private readonly string _apiUri;

    public RestClientFactory(IConfiguration config)
    {
      _apiUri = config.API_URI();
    }

    public IRestClient GetRestClient()
    {
      var client = new RestClient(_apiUri)
      {
        RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
      };

      return client;
    }
  }
}
