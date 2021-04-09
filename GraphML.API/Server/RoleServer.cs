using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class RoleServer : ItemServerBase<Role>, IRoleServer
  {
    public RoleServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<RoleServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Role)}";

    public async Task<IEnumerable<Role>> ByContactId(Guid id)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(RoleController.ByContactId)}", id.ToString()), 1, int.MaxValue);
      var retval = await GetResponse<IEnumerable<Role>>(request);

      return retval;
    }

    public async Task<IEnumerable<Role>> GetAll()
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(RoleController.GetAll)}"), 1, int.MaxValue);
      var retval = await GetResponse<IEnumerable<Role>>(request);

      return retval;
    }

    public async Task<IEnumerable<Role>> Get()
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(RoleController.Get)}"), 1, int.MaxValue);
      var retval = await GetResponse<IEnumerable<Role>>(request);

      return retval;
    }
  }
}
