using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
	public sealed class NodeItemAttributeDefinitionLogic : OwnedLogicBase<NodeItemAttributeDefinition>, INodeItemAttributeDefinitionLogic
	{
		public NodeItemAttributeDefinitionLogic(
			IHttpContextAccessor context,
			INodeItemAttributeDefinitionDatastore datastore,
			INodeItemAttributeDefinitionValidator validator,
			INodeItemAttributeDefinitionFilter filter) :
			base(context, datastore, validator, filter)
		{
		}
	}
}
