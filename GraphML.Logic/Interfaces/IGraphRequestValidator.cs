using FluentValidation;
using GraphML.Interfaces;

namespace GraphML.Logic.Interfaces
{
  public interface IGraphRequestValidator : IValidator<Graph>
  {
  }
}
