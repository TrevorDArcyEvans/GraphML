using Dapper.Contrib.Extensions;
using GraphML.Utils;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GraphML
{
  [Table(nameof(Edge))]
  public sealed class Edge : AttributedItem
  {
    [Required]
    [JsonProperty(nameof(Source))]
    public string Source { get; set; }

    [Required]
    [JsonProperty(nameof(Target))]
    public string Target { get; set; }

    [JsonProperty(nameof(Directed))]
    public bool Directed { get; set; }

    public Edge() :
      base()
    {
    }

    public Edge(string ownerId, string name, string source, string target) :
      base(ownerId, name)
    {
      Source = source.ThrowIfNullOrWhiteSpace(nameof(source));
      Target = target.ThrowIfNullOrWhiteSpace(nameof(target));
    }
  }
}
