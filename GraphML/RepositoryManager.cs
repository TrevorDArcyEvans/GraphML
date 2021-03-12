using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>A means to group a subset of <see cref="Repository"/> in an <see cref="Organisation"/> in some logical manner.</para>
	/// <para>For example, repositories could be grouped at a departmental level eg 'Financial Fraud' or 'Credit Control'</para>
	/// </summary>
	[Schema.Table(nameof(RepositoryManager))]
	public sealed class RepositoryManager : OwnedItem
	{
		public RepositoryManager() :
		  base()
		{
		}

		public RepositoryManager(Guid org, string name) :
		  base(org, org, name)
		{
		}
	}
}
