﻿using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using GraphML.Utils;
using Newtonsoft.Json;

namespace GraphML
{
  /// <summary>
  /// <para>Ultimate ancestor of all GraphML objects.</para>
  /// <para>Models something which can be persisted.</para>
  /// <para>Every item ultimately belongs to an <see cref="Organisation"/></para>
  /// </summary>
  public abstract class Item
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    [JsonProperty(nameof(Id))]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Unique identifier of owner Organisation
    /// </summary>
    [Required]
    [JsonProperty(nameof(OrganisationId))]
    public Guid OrganisationId { get; set; } = Guid.NewGuid();

    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    public Item()
    {
    }

    public Item(Guid org, string name)
    {
      Name = name.ThrowIfNullOrWhiteSpace(nameof(name));
      OrganisationId = org;
    }
  }
}
