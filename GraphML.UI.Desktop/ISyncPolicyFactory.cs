using Microsoft.Extensions.Logging;
using Polly;

namespace GraphML.UI.Desktop
{
  public interface ISyncPolicyFactory
  {
    ISyncPolicy Build(ILogger logger);
  }
}
