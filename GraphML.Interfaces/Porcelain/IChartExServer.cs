using System;
using System.Threading.Tasks;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartExServer
  {
    Task<ChartEx> ById(Guid id);
  }
}
