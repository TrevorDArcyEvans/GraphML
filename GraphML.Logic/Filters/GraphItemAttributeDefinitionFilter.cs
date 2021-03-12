using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
	public sealed class GraphItemAttributeDefinitionFilter : FilterBase<GraphItemAttributeDefinition>, IGraphItemAttributeDefinitionFilter
	{
		public GraphItemAttributeDefinitionFilter(
			IHttpContextAccessor context,
			IContactDatastore contactDatastore,
			IRoleDatastore roleDatastore) :
			base(context, contactDatastore, roleDatastore)
		{
		}
	}
}
