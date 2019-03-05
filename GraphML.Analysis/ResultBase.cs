using GraphML.Interfaces;

namespace GraphML.Analysis
{
  public abstract class ResultBase : IResult
  {
    public string Type => GetType().AssemblyQualifiedName;
  }
}
