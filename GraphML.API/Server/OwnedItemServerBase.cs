using Flurl;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace GraphML.API.Server
{
  public abstract class OwnedItemServerBase<T> : ItemServerBase<T>, IOwnedItemServerBase<T>
  {
    public OwnedItemServerBase(
      IRestClientFactory clientFactory,
      ILogger<OwnedItemServerBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    public IEnumerable<T> ByOwners(IEnumerable<string> ownerIds)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, "ByOwners"), ownerIds);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }
  }
}
