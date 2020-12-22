using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GraphML.API.Server
{
    public sealed class IdentityServer : ServerBase, IIdentityServer
    {
        public IdentityServer(
            IHttpContextAccessor httpContextAccessor, 
            IRestClientFactory clientFactory, 
            ILogger<IdentityServer> logger, 
            ISyncPolicyFactory policy) : 
            base(httpContextAccessor, clientFactory, logger, policy)
        {
        }

        protected override string ResourceBase { get; } = $"/api/Identity";

        public async Task<string> GetAPIUserClaimsJson()
        {
            var request = GetRequest(Url.Combine(ResourceBase, $"{nameof(IdentityController.GetAPIUserClaimsJson)}"));

            // have to get raw response as does not JSON deserialise
            var retval = await GetRawResponse(request);


            // format nicely
            var jobjs = JsonConvert.DeserializeObject<List<object>>(retval.Content);
            var json = JsonConvert.SerializeObject(jobjs, Formatting.Indented);

            return json;
        }
    }
}