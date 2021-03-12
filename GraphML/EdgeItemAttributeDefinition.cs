using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>Defines shape of information in a <see cref="EdgeItemAttribute"/></para>
	/// </summary>
	[Schema.Table(nameof(EdgeItemAttributeDefinition))]
	public sealed class EdgeItemAttributeDefinition : ItemAttributeDefinition
	{
		[Write(false)]
		public Guid RepositoryManagerId
		{
			get => OwnerId;
			set => OwnerId = value;
		}

		public EdgeItemAttributeDefinition() :
			base()
		{
		}

		public EdgeItemAttributeDefinition(Guid repoMgr, Guid orgId, string name, string dataType) :
			base(repoMgr, orgId, name, dataType)
		{
		}
	}
}
