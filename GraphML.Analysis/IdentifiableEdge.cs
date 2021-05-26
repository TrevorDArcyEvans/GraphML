using QuikGraph;
namespace GraphML.Analysis
{
    public sealed class IdentifiableEdge<T> : Edge<T>
    {
        public T Id { get; set; }

        public IdentifiableEdge(T id, T source, T target) : 
            base(source, target)
        {
            Id = id;
        }
    }
}
