using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
	public sealed class GraphItemAttributeDefinitionLogic : OwnedLogicBase<GraphItemAttributeDefinition>, IGraphItemAttributeDefinitionLogic
	{
		public GraphItemAttributeDefinitionLogic(
			IHttpContextAccessor context,
			IGraphItemAttributeDefinitionDatastore datastore,
			IGraphItemAttributeDefinitionValidator validator,
			IGraphItemAttributeDefinitionFilter filter) :
			base(context, datastore, validator, filter)
		{
		}
	}
}
