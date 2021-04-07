namespace GraphML.Datastore.Database.Importer.CSV
{
  using System.Linq;

  public abstract class ItemAttributeImportDefinition
  {
    public string Name { get; set; }
    public string DataType { get; set; }
    public string DateTimeFormat { get; set; }
    public int[] Columns { get; set; } = Enumerable.Empty<int>().ToArray();
  }
}
