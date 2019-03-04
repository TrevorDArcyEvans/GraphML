using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace GraphML.MessageQueue.ActiveMQ
{
  public sealed class RequestMessageReceiver : IRequestMessageReceiver
  {
    private IConfiguration _config { get; set; }

    public RequestMessageReceiver(
      IConfiguration config
      )
    {
      _config = config;
    }

    public string Receive()
    {
      var connecturi = new Uri(Settings.MESSAGE_QUEUE_URL(_config));
      var factory = new ConnectionFactory(connecturi);
      using (var connection = factory.CreateConnection())
      {
        using (var session = connection.CreateSession())
        {
          using (var destination = SessionUtil.GetQueue(session, Settings.MESSAGE_QUEUE_NAME(_config)))
          {
            using (var consumer = session.CreateConsumer(destination))
            {
              connection.Start();
              var msg = (ITextMessage)consumer.Receive(TimeSpan.FromSeconds(Settings.MESSAGE_QUEUE_POLL_INTERVAL_S(_config)));
              return msg.Text;
            }
          }
        }
      }
    }
  }
}
