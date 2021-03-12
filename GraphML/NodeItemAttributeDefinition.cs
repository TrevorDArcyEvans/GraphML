using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>Defines shape of information in a <see cref="NodeItemAttribute"/></para>
	/// </summary>
	[Schema.Table(nameof(NodeItemAttributeDefinition))]
	public sealed class NodeItemAttributeDefinition : ItemAttributeDefinition
	{
		[Write(false)]
		public Guid RepositoryManagerId
		{
			get => OwnerId;
			set => OwnerId = value;
		}

		public NodeItemAttributeDefinition() :
			base()
		{
		}

		public NodeItemAttributeDefinition(Guid repoMgr, Guid orgId, string name, string dataType) :
			base(repoMgr, orgId, name, dataType)
		{
		}
	}
}
