using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
	public sealed class RepositoryItemAttributeDefinitionLogic : OwnedLogicBase<RepositoryItemAttributeDefinition>, IRepositoryItemAttributeDefinitionLogic
	{
		public RepositoryItemAttributeDefinitionLogic(
			IHttpContextAccessor context,
			IRepositoryItemAttributeDefinitionDatastore datastore,
			IRepositoryItemAttributeDefinitionValidator validator,
			IRepositoryItemAttributeDefinitionFilter filter) :
			base(context, datastore, validator, filter)
		{
		}
	}
}
