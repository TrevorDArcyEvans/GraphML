using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GraphML
{
  [Table(nameof(RepositoryManager))]
  public sealed class RepositoryManager : OwnedItem
  {
    /// <summary>
    /// Unique identifier of organisation
    /// </summary>
    [Required]
    [JsonProperty(nameof(OrganisationId))]
    public string OrganisationId { get; set; }

    public RepositoryManager() :
      base()
    {
    }

    public RepositoryManager(string orgId, string name) :
      base(Guid.Empty.ToString(), name)
    {
      OrganisationId = orgId;
    }
  }
}
