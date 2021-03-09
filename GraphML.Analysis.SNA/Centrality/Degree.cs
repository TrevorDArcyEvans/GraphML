using QuikGraph;
using System;
using System.Linq;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class Degree<TVertex, TEdge> : ICentralityDegreeAlgorithm<TVertex> where TEdge : IEdge<TVertex> where TVertex : IEquatable<TVertex>
  {
    private readonly IEdgeSet<TVertex, TEdge> _edgeSet;
    private readonly Func<TEdge, double> _weights;

    public event DegreeResultAction<TVertex> VertexResult;

    public Degree(IEdgeSet<TVertex, TEdge> edgeSet, Func<TEdge, double> weights)
    {
      _edgeSet = edgeSet;
      _weights = weights;
    }

    public void Compute()
    {
      var nodes = _edgeSet.Edges.SelectMany(e => new[] { e.Source, e.Target }).Distinct();

      foreach (var node in nodes)
      {
        var inEdges = _edgeSet.Edges.Where(e => e.Target.Equals(node));
        var inDegree = inEdges.Sum(e => _weights(e));
        var outEdges = _edgeSet.Edges.Where(e => e.Source.Equals(node));
        var outDegree = outEdges.Sum(e => _weights(e));

        OnVertexResult(new DegreeVertexResult<TVertex>(node, inDegree, outDegree));
      }
    }
    private void OnVertexResult(DegreeVertexResult<TVertex> result)
    {
      VertexResult?.Invoke(result);
    }
  }
}
