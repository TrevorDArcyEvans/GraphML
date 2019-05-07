using System;
using System.Windows.Forms;

namespace GraphML.UI.Desktop
{
  public sealed class AutoCursor : IDisposable
  {
    private readonly Cursor _oldCursor;

    public AutoCursor()
    {
      _oldCursor = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
    }

    public void Dispose()
    {
      Cursor.Current = _oldCursor;
    }
  }
}
