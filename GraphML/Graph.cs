using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
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

    public Graph(Guid repo, string name) :
      base(repo, name)
    {
    }
  }
}
