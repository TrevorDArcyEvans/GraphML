using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(GraphEdge))]
  public sealed class GraphEdge : GraphItem
  {
    [Required]
    [JsonProperty(nameof(SourceId))]
    public Guid SourceId { get; set; }

    [Required]
    [JsonProperty(nameof(TargetId))]
    public Guid TargetId { get; set; }

    public GraphEdge() :
      base()
    {
    }

    public GraphEdge(Guid graph,Guid repositoryItem,  string name, Guid source, Guid target) :
      base(graph, repositoryItem, name)
    {
      SourceId = source;
      TargetId = target;
    }
  }
}
