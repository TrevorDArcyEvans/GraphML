using GraphML.Interfaces;
using GraphML.Logic.Filters;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Tests
{
  public sealed class DummyFilter : FilterBase<DummyItem>
  {
    public DummyFilter(
      IHttpContextAccessor context, 
      IContactDatastore contactDatastore) : 
      base(context, contactDatastore)
    {
    }
  }
}