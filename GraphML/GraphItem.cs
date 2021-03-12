using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace GraphML
{
	/// <summary>
	/// Something which is in a <see cref="Graph"/>, either a <see cref="GraphNode"/> or a <see cref="GraphEdge"/>
	/// </summary>
	public abstract class GraphItem : OwnedItem
	{
		[Write(false)]
		public Guid GraphId
		{
			get => OwnerId;
			set => OwnerId = value;
		}

		[Required]
		[JsonProperty(nameof(RepositoryItemId))]
		public Guid RepositoryItemId { get; set; }

		protected GraphItem() :
		  base()
		{
		}

		protected GraphItem(Guid graph, Guid orgId, Guid repositoryItem, string name) :
		  base(graph, orgId, name)
		{
			RepositoryItemId = repositoryItem;
		}
	}
}
