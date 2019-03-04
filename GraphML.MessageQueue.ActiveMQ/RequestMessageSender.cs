using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace GraphML.MessageQueue.ActiveMQ
{
  public sealed class RequestMessageSender : IRequestMessageSender
  {
    private readonly IConfiguration _config;

    public RequestMessageSender(
      IConfiguration config
      )
    {
      _config = config;
    }

    public void Send(string json)
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
              var msg = session.CreateTextMessage(json);

              producer.Send(msg);
            }
          }
        }
      }
    }
  }
}
