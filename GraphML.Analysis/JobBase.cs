using GraphML.Interfaces;

namespace GraphML.Analysis
{
  public abstract class JobBase : IJob
  {
    public abstract void Run(IRequest req);
  }
}
