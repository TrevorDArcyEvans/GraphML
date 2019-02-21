using GraphML.Utils;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Edge))]
  public sealed class Edge : GraphItem
  {
    [Required]
    [JsonProperty(nameof(SourceId))]
    public string SourceId { get; set; }

    [Required]
    [JsonProperty(nameof(TargetId))]
    public string TargetId { get; set; }

    public Edge() :
      base()
    {
    }

    public Edge(string ownerId, string name, string source, string target) :
      base(ownerId, name)
    {
      SourceId = source.ThrowIfNullOrWhiteSpace(nameof(source));
      TargetId = target.ThrowIfNullOrWhiteSpace(nameof(target));
    }
  }
}
