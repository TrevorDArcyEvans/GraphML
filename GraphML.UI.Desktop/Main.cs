using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GraphML.UI.Desktop
{
  public partial class Main : Form
  {
    private readonly IRepositoryManagerServer _repoMgrServer;
    private readonly IRepositoryServer _repoServer;

    public Main()
    {
      InitializeComponent();
    }

    public Main(IServiceProvider sp) :
      this()
    {
      _repoMgrServer = sp.GetService<IRepositoryManagerServer>();
      _repoServer = sp.GetService<IRepositoryServer>();
    }

    private void Overview_DoubleClick(object sender, EventArgs e)
    {
      using (new AutoCursor())
      {
        var selNode = Overview.SelectedNode;

        if (selNode.Parent == null)
        {
          selNode.Nodes.Clear();
          var repoMgrs = _repoMgrServer.GetAll();
          var childNodes = repoMgrs.Select(x => new TreeNode(x.Name) { Tag = x });
          selNode.Nodes.AddRange(childNodes.ToArray());
          selNode.ExpandAll();
        }

        if (selNode.Tag is RepositoryManager repoMgr)
        {
          selNode.Nodes.Clear();
          var repos = _repoServer.ByOwners(new[] { repoMgr.Id });
          var childNodes = repos.Select(x => new TreeNode(x.Name) { Tag = x });
          selNode.Nodes.AddRange(childNodes.ToArray());
          selNode.ExpandAll();
        }

        if (selNode.Tag is Repository repo)
        {
          // TODO   expand nodes+edges?
        }
      }
    }

    private void CmdImport_Click(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var repo = (Repository)selNode.Tag;
      var import = new Import(repo);
      import.ShowDialog();
    }

    private void Overview_MouseDown(object sender, MouseEventArgs e)
    {
      // Make sure this is the right button.
      if (e.Button != MouseButtons.Right)
      {
        return;
      }

      // Select this node.
      var selNode = Overview.GetNodeAt(e.X, e.Y);
      Overview.SelectedNode = selNode;

      // See if we got a node.
      if (selNode == null)
      {
        return;
      }

      // See what kind of object this is and
      // display the appropriate popup menu.
      if (selNode.Tag is Repository)
      {
        RepositoryMenu.Show(Overview, new Point(e.X, e.Y));
      }
    }
  }
}
