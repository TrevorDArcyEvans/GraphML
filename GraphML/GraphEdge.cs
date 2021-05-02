using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>An <see cref="Edge"/> which appears in a <see cref="Graph"/>.</para>
  /// <remarks>Name may be different to that of underlying Edge</remarks>
  /// </summary>
  [Schema.Table(nameof(GraphEdge))]
  public sealed class GraphEdge : GraphItem
  {
    [Required]
    [JsonProperty(nameof(GraphSourceId))]
    public Guid GraphSourceId { get; set; }

    [Required]
    [JsonProperty(nameof(GraphTargetId))]
    public Guid GraphTargetId { get; set; }

    public GraphEdge() :
      base()
    {
    }

    public GraphEdge(
      Guid graph,
      Guid orgId,
      Guid repositoryItem,
      string name,
      Guid source,
      Guid target) :
      base(graph, orgId, repositoryItem, name)
    {
      GraphSourceId = source;
      GraphTargetId = target;
    }
  }
}
