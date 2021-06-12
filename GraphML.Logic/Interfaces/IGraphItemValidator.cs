using FluentValidation;

namespace GraphML.Logic.Interfaces
{
  public interface IGraphItemValidator<T> : IValidator<T> where T : GraphItem
  {
  }
}
