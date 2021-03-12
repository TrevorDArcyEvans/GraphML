using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
	public sealed class RepositoryItemAttributeDefinitionValidator : OwnedValidatorBase<RepositoryItemAttributeDefinition>, IRepositoryItemAttributeDefinitionValidator
	{
		public RepositoryItemAttributeDefinitionValidator(
			IHttpContextAccessor context,
			IContactDatastore contactDatastore,
			IRoleDatastore roleDatastore) :
			base(context, contactDatastore, roleDatastore)
		{
		}
	}
}
