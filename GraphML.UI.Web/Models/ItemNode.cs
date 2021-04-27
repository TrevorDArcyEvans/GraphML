using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace GraphML.UI.Web
{
  public class ItemNode : NodeModel
  {
    public ItemNode(string id, string name, Point pos) :
      base(id, pos)
    {
      Name = name;
    }

    public string Name { get; set; }
  }
}
