using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>A link connecting two <see cref="Node"/>.</para>
	/// <para>An Edge may have a 'weight/s' (or other properties) associated with it via an <see cref="EdgeItemAttribute"/></para>
	/// <para>An Edge is not directed 'per se'; this is set on the <see cref="Graph"/></para>
	/// </summary>
	[Schema.Table(nameof(Edge))]
	public sealed class Edge : RepositoryItem
	{
		[Required]
		[JsonProperty(nameof(SourceId))]
		public Guid SourceId { get; set; }

		[Required]
		[JsonProperty(nameof(TargetId))]
		public Guid TargetId { get; set; }

		public Edge() :
		  base()
		{
		}

		public Edge(Guid repo, Guid orgId, string name, Guid source, Guid target) :
		  base(repo, orgId, name)
		{
			SourceId = source;
			TargetId = target;
		}
	}
}
