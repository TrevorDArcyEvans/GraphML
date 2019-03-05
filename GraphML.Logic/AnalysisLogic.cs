using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GraphML.Logic
{
  public sealed class AnalysisLogic : IAnalysisLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IRequestMessageSender _sender;

    public AnalysisLogic(
      IHttpContextAccessor context,
      IRequestMessageSender sender)
    {
      _context = context;
      _sender = sender;
    }

    public void Degree(IDegreeRequest req)
    {
      // TODO   validation
      Send(req);
    }

    public void Closeness(IClosenessRequest req)
    {
      // TODO   validation
      Send(req);
    }

    public void Betweenness(IBetweennessRequest req)
    {
      // TODO   validation
      Send(req);
    }

    private void Send(IRequest req)
    {
      var jsonReq = JsonConvert.SerializeObject(req);
      _sender.Send(jsonReq);
    }
  }
}
