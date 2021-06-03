using MatBlazor;

namespace GraphML.UI.Web.Widgets
{
  public class MatTableEx<T> : MatTable<T>
  {
    public string GetSearchTerm() => SearchTerm;
  }
}
