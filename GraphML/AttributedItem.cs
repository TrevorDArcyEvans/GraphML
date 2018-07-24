namespace GraphML
{
  public abstract class AttributedItem : OwnedItem
  {
    public AttributedItem() :
      base()
    {
    }

    public AttributedItem(string ownerId, string name) :
      base(ownerId, name)
    {
    }
  }
}
