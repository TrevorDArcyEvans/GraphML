using QuickGraph;

namespace GraphML.Analysis.RankedShortestPath
{
  public interface IFindShortestPathsAlgorithm<TVertex, TEdge> where TEdge : IEdge<TVertex>
  {
    FindShortestPathsResults<TEdge> Compute(TVertex rootVertex, TVertex goalVertex);
  }
}
