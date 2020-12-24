using System.Collections.Generic;
using FluentValidation;

namespace GraphML.Logic.Interfaces
{
  public interface IGraphNodesRequestValidator : IValidator<IEnumerable<GraphNode>>
  {
  }
}
