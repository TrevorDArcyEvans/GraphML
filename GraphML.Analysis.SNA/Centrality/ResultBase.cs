using GraphML.Interfaces;

namespace GraphML.Analysis.SNA.Centrality
{
  public abstract class ResultBase : IResult
  {
    public string Type => GetType().AssemblyQualifiedName;
  }
}
