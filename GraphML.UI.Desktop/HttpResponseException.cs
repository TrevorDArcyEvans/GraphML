using System;
using System.Net;

namespace GraphML.UI.Desktop
{
  public sealed class HttpResponseException : Exception
  {
    public HttpStatusCode StatusCode { get; }

    public HttpResponseException(HttpStatusCode statusCode, string message) :
      base(message)
    {
      StatusCode = statusCode;
    }
  }
}
