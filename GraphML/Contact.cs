using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>A person identified by their email address.</para>
	/// <para>The email address (Name) is used to link authentication (IdentityServer) to <see cref="Role"/>.</para>
	/// </summary>
	[Schema.Table(nameof(Contact))]
	public sealed class Contact : OwnedItem
	{
		public Contact() :
		  base()
		{
		}

		public Contact(Guid org, string email) :
		  base(org, org, email)
		{
		}
	}
}
