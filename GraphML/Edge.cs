using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
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

    public Edge(Guid repository, Guid orgId, string name, Guid source, Guid target) :
      base(repository, orgId, name)
    {
      SourceId = source;
      TargetId = target;
    }
  }
}
