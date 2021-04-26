using FluentValidation;
using GraphML.Porcelain;

namespace GraphML.Logic.Interfaces.Porcelain
{
  public interface IChartExValidator : IValidator<ChartEx>
  {
  }
  public interface IChartNodeExValidator : IValidator<ChartNodeEx>
  {
  }
  public interface IChartEdgeExValidator : IValidator<ChartEdgeEx>
  {
  }
}
