using System.Collections.Generic;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  // IGraphNodesRequestValidator --> all GraphNodes are in same graph
  //    IGraphNodeDatastore.ByIds --> GraphNode.GraphId[]
  public sealed class GraphNodesRequestValidator : ValidatorBase<IEnumerable<GraphNode>>, IGraphNodesRequestValidator
  {
    public GraphNodesRequestValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
