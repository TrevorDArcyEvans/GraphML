using System.Collections.Generic;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
	public sealed class NodeLogic : OwnedLogicBase<Node>, INodeLogic
	{
		public NodeLogic(
		  IHttpContextAccessor context,
		  INodeDatastore datastore,
		  INodeValidator validator,
		  INodeFilter filter) :
		  base(context, datastore, validator, filter)
		{
		}

		public IEnumerable<Node> GetParents(Node entity, int pageIndex, int pageSize)
		{
			// TODO		GetParents
			throw new System.NotImplementedException();
		}
	}
}
