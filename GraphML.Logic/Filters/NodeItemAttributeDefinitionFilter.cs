using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
	public sealed class NodeItemAttributeDefinitionFilter : FilterBase<NodeItemAttributeDefinition>, INodeItemAttributeDefinitionFilter
	{
		public NodeItemAttributeDefinitionFilter(
			IHttpContextAccessor context,
			IContactDatastore contactDatastore,
			IRoleDatastore roleDatastore) :
			base(context, contactDatastore, roleDatastore)
		{
		}
	}
}
