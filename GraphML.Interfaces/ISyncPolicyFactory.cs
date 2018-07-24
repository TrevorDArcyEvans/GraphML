using Microsoft.Extensions.Logging;
using Polly;

namespace GraphML.Interfaces
{
  public interface ISyncPolicyFactory
  {
    ISyncPolicy Build(ILogger logger);
  }
}
