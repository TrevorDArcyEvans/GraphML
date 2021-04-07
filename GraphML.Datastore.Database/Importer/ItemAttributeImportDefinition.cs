﻿using System.Linq;

namespace GraphML.Datastore.Database.Importer
{
  public abstract class ItemAttributeImportDefinition
  {
    public string Name { get; set; }
    public string DataType { get; set; }
    public string DateTimeFormat { get; set; }
    public int[] Columns { get; set; } = Enumerable.Empty<int>().ToArray();
  }
}