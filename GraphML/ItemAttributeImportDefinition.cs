using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraphML.Utils;
using Newtonsoft.Json;

namespace GraphML.Datastore.Database.Importer
{
  public abstract class ItemAttributeImportDefinition
  {
    /// <summary>
    /// Name of <see cref="ItemAttributeDefinition"/> to be displayed to user.
    /// </summary>
    [Required]
    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    /// <summary>
    /// Underlying data type.  Currently supported data types:
    /// <list type="bullet">
    /// <item>string</item>
    /// <item>bool</item>
    /// <item>int</item>
    /// <item>double</item>
    /// <item>DateTime</item>
    /// <item><see cref="DateTimeInterval"/></item>
    /// </list>
    /// </summary>
    [Required]
    [JsonProperty(nameof(DataType))]
    public string DataType { get; set; }

    /// <summary>
    /// <see cref="DateTime"/> format to use when parsing.  Currently supported formats:
    /// <list type="bullet">
    /// <item>all standard and custom .NET DateTime formats</item>
    /// <item>"SecondsSinceUnixEpoch"</item>
    /// </list>
    /// <remarks>
    /// DateTime is assumed to be in UTC.<br/>
    /// If not specified, will try generic parse using invariant culture.
    /// </remarks>
    /// </summary>
    [Required]
    [JsonProperty(nameof(DateTimeFormat))]
    public string DateTimeFormat { get; set; }

    /// <summary>
    /// Zero based array column index for attribute value
    /// <remarks>
    /// For <see cref="DateTimeInterval"/>:<br/>
    /// first index is Start<br/>
    /// second index is End
    /// </remarks>
    /// </summary>
    [Required]
    [JsonProperty(nameof(Columns))]
    public int[] Columns { get; set; } = Enumerable.Empty<int>().ToArray();
  }
}
