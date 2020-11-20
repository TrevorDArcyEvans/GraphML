using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
    public sealed class GraphNodeValidator : OwnedValidatorBase<GraphNode>, IGraphNodeValidator
    {
        public GraphNodeValidator(IHttpContextAccessor context) :
            base(context)
        {
        }
    }
}