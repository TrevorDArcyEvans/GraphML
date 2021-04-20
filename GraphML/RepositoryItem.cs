using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace GraphML
{
  /// <summary>
  /// Something which is in a <see cref="Repository"/>, either a <see cref="Node"/> or an <see cref="Edge"/>
  /// </summary>
  public abstract class RepositoryItem : OwnedItem
  {
    [JsonProperty(nameof(NextId))]
    public Guid NextId { get; set; } = Guid.Empty;

    [Write(false)]
    public Guid RepositoryId
    {
      get => OwnerId;

      set => OwnerId = value;
    }

    public RepositoryItem() :
      base()
    {
    }

    protected RepositoryItem(Guid repo, Guid orgId, string name) :
      base(repo, orgId, name)
    {
    }
  }
}
