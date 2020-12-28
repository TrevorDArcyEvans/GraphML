using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(ContactsRoles))]
  public sealed class ContactsRoles
  {
    [Required]
    [JsonProperty(nameof(ContactId))]
    public Guid ContactId { get; set; }

    [Required]
    [JsonProperty(nameof(RoleId))]
    public Guid RoleId { get; set; }
  }
}
