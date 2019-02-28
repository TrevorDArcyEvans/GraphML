namespace GraphML.Analysis
{
  public abstract class JobBase : IJob
  {
    public abstract void Run(RequestBase req);
  }
}
