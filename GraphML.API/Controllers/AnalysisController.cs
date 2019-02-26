using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using GraphML.Analysis.SNA.Centrality;
using GraphML.API.Attributes;
using GraphML.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Analysis
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class AnalysisController : ControllerBase
  {
    private readonly IConfiguration _config;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="config">configuration</param>
    public AnalysisController(
      IConfiguration config)
    {
      _config = config;
    }

    /// <summary>
    /// Calculate SNA 'Degree' for specified graph
    /// </summary>
    /// <param name="graphId">Unique identifier of graph</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route(nameof(Degree))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult Degree([Required] string graphId)
    {
      var connecturi = new Uri(Settings.MESSAGE_QUEUE_URL(_config));
      var factory = new ConnectionFactory(connecturi);
      using (var connection = factory.CreateConnection())
      {
        using (var session = connection.CreateSession())
        {
          using (var destination = SessionUtil.GetQueue(session, Settings.MESSAGE_QUEUE_NAME(_config)))
          {
            using (var producer = session.CreateProducer(destination))
            {
              // Start the connection so that messages will be processed.
              connection.Start();

              producer.DeliveryMode = MsgDeliveryMode.Persistent;

              // Send a message
              var req = new DegreeRequest
              {
                CorrelationId = Guid.NewGuid().ToString(),
                GraphId = graphId
              };
              var json = JsonConvert.SerializeObject(req);
              var msg = session.CreateTextMessage(json);
              msg.NMSCorrelationID = req.CorrelationId;

              producer.Send(msg);

              return new OkObjectResult(msg.NMSCorrelationID);
            }
          }
        }
      }
    }
  }
}
