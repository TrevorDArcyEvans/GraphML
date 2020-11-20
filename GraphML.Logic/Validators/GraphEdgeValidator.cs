using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
    public sealed class GraphEdgeValidator : OwnedValidatorBase<GraphEdge>, IGraphEdgeValidator
    {
        public GraphEdgeValidator(IHttpContextAccessor context) :
            base(context)
        {
        }
    }
}