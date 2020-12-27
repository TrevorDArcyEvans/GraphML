using System;

namespace GraphML
{
    [System.ComponentModel.DataAnnotations.Schema.Table(nameof(EdgeNode))]
    public sealed class EdgeNode : GraphItem
    {
        public EdgeNode() :
            base()
        {
        }

        public EdgeNode(Guid graph, Guid orgId, Guid repositoryItem, string name) :
            base(graph, orgId, repositoryItem, name)
        {
        }
    }
}