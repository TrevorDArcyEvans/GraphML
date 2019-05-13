using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML.Views
{
  [Schema.Table(nameof(GraphView))]
  public sealed class ListView : GraphView
  {
    public ListView() :
      base()
    {
    }

    public ListView(string graphId, string name) :
      base(graphId, name)
    {
      ViewType = typeof(ListView).ToString();
    }
  }
}
