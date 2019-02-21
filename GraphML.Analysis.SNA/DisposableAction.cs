using System;

namespace GraphML.Analysis.SNA
{
  public sealed class DisposableAction : IDisposable
  {
    private Action _action;

    public DisposableAction(Action action)
    {
      _action = action;
    }

    public void Dispose()
    {
      var action = _action;
      _action = null;
      action?.Invoke();
    }
  }
}
