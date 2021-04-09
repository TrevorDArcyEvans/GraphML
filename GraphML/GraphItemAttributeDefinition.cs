using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>Defines shape of information in a <see cref="GraphItemAttribute"/></para>
  /// </summary>
  [Schema.Table(nameof(GraphItemAttributeDefinition))]
  public sealed class GraphItemAttributeDefinition : ItemAttributeDefinition
  {
    [Write(false)]
    public Guid RepositoryManagerId
    {
      get => OwnerId;

      set => OwnerId = value;
    }

    public GraphItemAttributeDefinition() :
      base()
    {
    }

    public GraphItemAttributeDefinition(Guid repoMgr, Guid orgId, string name, string dataType) :
      base(repoMgr, orgId, name, dataType)
    {
    }
  }
}
