﻿namespace GraphML.Interfaces
{
  public interface IAnalysisLogic
  {
    void Degree(IDegreeRequest req);
    void Closeness(IClosenessRequest req);
    void Betweenness(IBetweennessRequest req);
  }
}
