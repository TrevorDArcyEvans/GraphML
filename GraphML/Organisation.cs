using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>Typically a company, organisation or other legal entity /// in which people work together.</para>
  /// <example>
  /// <list type="bullet">
  /// <item>police force</item>
  /// <item>GCHQ</item>
  /// <item>FBI</item>
  /// <item>military</item>
  /// <item>bank</item>
  /// </list>
  /// </example>
  /// <remarks>
  /// Id and OrganisationId **must** be the same
  /// </remarks>
  /// </summary>
  [Schema.Table(nameof(Organisation))]
  public sealed class Organisation : Item
  {
    public Organisation() :
      base()
    {
    }

    public Organisation(Guid org, string name) :
      base(org, name)
    {
      Id = org;
    }
  }
}