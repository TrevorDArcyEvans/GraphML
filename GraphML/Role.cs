using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>The function performed by a <see cref="Contact"/> in the context of GraphML.</para>
	/// <para>There are several, predefined functions in <see cref="Roles"/></para>
	/// <para>A <see cref="Contact"/> may have one or more <see cref="Roles"/></para>
	/// </summary>
	[Schema.Table(nameof(Role))]
	public sealed class Role : Item
	{
		public Role() :
		  base()
		{
		}

		public Role(Guid orgId, string name) :
		  base(orgId, name)
		{
		}
	}
}
