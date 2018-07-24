using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using GraphML.Utils;
using Newtonsoft.Json;

namespace GraphML
{
  public abstract class Item
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    [JsonProperty(nameof(Id))]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    public Item()
    {
    }

    public Item(string name)
    {
      Name = name.ThrowIfNullOrWhiteSpace(nameof(name));
    }
  }
}
