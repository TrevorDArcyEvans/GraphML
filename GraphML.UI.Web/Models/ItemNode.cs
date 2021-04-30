using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace GraphML.UI.Web.Models
{
  public class ItemNode : NodeModel
  {
    /// <summary>
    /// A <see cref="Node"/> displayed in a <see cref="Chart"/>
    /// </summary>
    /// <param name="id">Unique identifier of underlying <see cref="Node"/></param>
    /// <param name="name">Name of <see cref="Node"/></param>
    /// <param name="pos">Location of <see cref="ItemNode"/> on <see cref="Chart"/></param>
    public ItemNode(string id, string name, Point pos) :
      base(id, pos)
    {
      Name = name;
    }

    public string Name { get; set; }
  }
}
