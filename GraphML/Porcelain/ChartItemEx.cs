using System;

namespace GraphML.Porcelain
{
  public abstract class ChartItemEx : OwnedItem
  {
    public Guid GraphItemId { get; set; }
    public Guid RepositoryItemId { get; set; }
  }
}
