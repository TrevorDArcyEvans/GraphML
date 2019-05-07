using Flurl;
using RestSharp;
using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public sealed class RepositoryManagerServer : ServerBase<RepositoryManager>
  {
    public RepositoryManagerServer(IRestClient client) :
      base(client)
    {
    }

    protected override string ResourceBase { get; } = "/api/RepositoryManager";

    public IEnumerable<RepositoryManager> GetAll()
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, "GetAll"));
      var retval = GetResponse<IEnumerable<RepositoryManager>>(request);

      return retval;
    }
  }
}
