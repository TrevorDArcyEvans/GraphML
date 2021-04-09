using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>Defines shape of information in a <see cref="RepositoryItemAttribute"/></para>
  /// </summary>
  [Schema.Table(nameof(RepositoryItemAttributeDefinition))]
  public sealed class RepositoryItemAttributeDefinition : ItemAttributeDefinition
  {
    [Write(false)]
    public Guid RepositoryManagerId
    {
      get => OwnerId;

      set => OwnerId = value;
    }

    public RepositoryItemAttributeDefinition() :
      base()
    {
    }

    public RepositoryItemAttributeDefinition(Guid repoMgr, Guid orgId, string name, string dataType) :
      base(repoMgr, orgId, name, dataType)
    {
    }
  }
}
