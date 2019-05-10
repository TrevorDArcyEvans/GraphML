﻿using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Repository))]
  public sealed class Repository : AttributedItem
  {
    public Repository() :
    base()
    {
    }

    public Repository(string repoMgrId, string name) :
      base(repoMgrId, name)
    {
    }
  }
}
