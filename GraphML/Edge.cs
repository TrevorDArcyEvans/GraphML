using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Edge))]
  public sealed class Edge : GraphItem
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

    public Edge(Guid graphId, string name, Guid source, Guid target) :
      base(graphId, name)
    {
      SourceId = source;
      TargetId = target;
    }
  }
}
