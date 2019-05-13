using Microsoft.Extensions.Logging;
using Polly;

namespace GraphML.API.Server
{
  public interface ISyncPolicyFactory
  {
    ISyncPolicy Build(ILogger logger);
  }
}
