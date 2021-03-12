using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	/// <summary>
	/// <para>A subset of <see cref="Node"/> and <see cref="Edge"/> from a <see cref="Repository"/> which have been extracted for separate analysis.</para>
	/// <para>A Graph may be directed; in contract to a <see cref="Repository"/>, which has no notion of direction.</para>
	/// </summary>
	[Schema.Table(nameof(Graph))]
	public sealed class Graph : OwnedItem
	{
		[JsonProperty(nameof(Directed))]
		public bool Directed { get; set; } = true;

		[Write(false)]
		public Guid RepositoryId
		{
			get => OwnerId;
			set => OwnerId = value;
		}

		public Graph() :
		  base()
		{
		}

		public Graph(Guid repo, Guid orgId, string name) :
		  base(repo, orgId, name)
		{
		}
	}
}
