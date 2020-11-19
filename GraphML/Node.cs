﻿using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Node))]
  public sealed class Node : RepositoryItem
  {
    public Node() :
      base()
    {
    }

    public Node(Guid repository, string name) :
      base(repository, name)
    {
    }
  }
}
