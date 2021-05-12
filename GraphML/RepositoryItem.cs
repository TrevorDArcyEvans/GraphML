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
    /// <summary>
    /// Optional unique identifier of child item.
    /// This is used to preserve the history of an
    /// item when merging items.
    /// <remarks>
    /// This has to be nullable as the default value
    /// for a GUID (Guid.Empty) will violate the
    /// referential integrity constraints in our
    /// underlying database.
    /// </remarks>
    /// </summary>
    [JsonProperty(nameof(NextId))]
    public Guid? NextId { get; set; }

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
