using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>A vertex representing something of interest.</para>
	/// <para>A Node may be connected to zero or one other Nodes by an <see cref="Edge"/></para>
  /// <para>A Node may have properties associated with it via an <see cref="NodeItemAttribute"/></para>
	/// </summary>
	[Schema.Table(nameof(Node))]
	public sealed class Node : RepositoryItem
	{
		public Node() :
		  base()
		{
		}

		public Node(Guid repository, Guid orgId, string name) :
		  base(repository, orgId, name)
		{
		}
	}
}
