using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GraphML.UI.Desktop
{
  public class Server
  {
    private readonly RestClient _client;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings();

    public Server(
      string serverUrl,
      string userName,
      string password
      )
    {
      _client = new RestClient(serverUrl)
      {
        Authenticator = new HttpBasicAuthenticator(userName, password)
      };
      _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
    }
    private string ResourceBase { get; } = "/api/RepositoryManager";

    public string RepositoryManager_GetAll()
    {
      var request = GetAllRequest($"{ResourceBase}/GetAll");
      //var retval = GetResponse<PaginatedList<Frameworks>>(request);
      var resp = GetRawResponse(request);

      return resp.Content;
    }

    protected IRestRequest GetRequest(string path)
    {
      var request = new RestRequest(path)
      {
        Method = Method.GET
      };
      request.AddHeader("Content-Type", "application/json");

      return request;
    }

    protected IRestRequest GetAllRequest(string path)
    {
      var request = GetRequest(path);
      AddGetAllParameters(request);

      return request;
    }

    private static void AddGetAllParameters(IRestRequest request)
    {
      const int StartPageIndex = 1;
      const int GetAllPageSize = int.MaxValue;

      request.AddQueryParameter("PageIndex", StartPageIndex.ToString(CultureInfo.InvariantCulture));
      request.AddQueryParameter("PageSize", GetAllPageSize.ToString(CultureInfo.InvariantCulture));
    }

    protected TOther GetResponse<TOther>(IRestRequest request)
    {
      var resp = GetRawResponse(request);
      var retval = JsonConvert.DeserializeObject<TOther>(resp.Content, _settings);

      return retval;
    }

    protected IRestResponse GetRawResponse(IRestRequest request)
    {
      var resp = _client.Execute(request);

      return resp;
    }
  }
}
