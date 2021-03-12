using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
	public sealed class EdgeItemAttributeDefinitionValidator : OwnedValidatorBase<EdgeItemAttributeDefinition>, IEdgeItemAttributeDefinitionValidator
	{
		public EdgeItemAttributeDefinitionValidator(
			IHttpContextAccessor context,
			IContactDatastore contactDatastore,
			IRoleDatastore roleDatastore) :
			base(context, contactDatastore, roleDatastore)
		{
		}
	}
}
