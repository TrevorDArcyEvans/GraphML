using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace GraphML.API.Server
{
  public sealed class SyncPolicyFactory : ISyncPolicyFactory
  {
    public ISyncPolicy Build(ILogger logger)
    {
      return Policy.Handle<Exception>()
        .WaitAndRetry(3, // We can also do this with WaitAndRetryForever... but chose WaitAndRetry this time.
          attempt => TimeSpan.FromSeconds(0.1 * Math.Pow(2, attempt)), // Back off!  2, 4, 8, 16 etc times 1/4-second
            (exception, calculatedWaitDuration) =>  // Capture some info for logging!
            {
              logger.LogError($"Error in {logger.ToString()} after {calculatedWaitDuration.TotalSeconds.ToString()}: {exception.Message}");
            });
    }
  }
}
