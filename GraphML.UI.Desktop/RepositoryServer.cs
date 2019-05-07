using RestSharp;

namespace GraphML.UI.Desktop
{
  public sealed class RepositoryServer : ServerBase<Repository>
  {
    public RepositoryServer(IRestClient client) :
      base(client)
    {
    }

    protected override string ResourceBase { get; } = "/api/Repository";
  }
}
