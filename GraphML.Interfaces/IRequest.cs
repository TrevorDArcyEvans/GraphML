using System;

namespace GraphML.Interfaces
{
  public interface IRequest
  {
    /// <summary>
    /// Assembly containing type of request
    /// Used to deserialise request
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Assembly implementing IJob which will run this job
    /// </summary>
    string JobType { get; }

    /// <summary>
    /// Unique reference for this request
    /// </summary>
    Guid CorrelationId { get; set; }

    /// <summary>
    /// Person making this request
    /// </summary>
    string ContactId { get; set; }

    /// <summary>
    /// Description of this request
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// UTC time when request was made
    /// </summary>
    DateTime CreatedOnUtc { get; set; }
  }
}
