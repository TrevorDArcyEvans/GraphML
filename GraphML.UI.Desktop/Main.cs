using Microsoft.Extensions.DependencyInjection;
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
    private readonly IGraphServer _graphServer;

    public Main()
    {
      InitializeComponent();
    }

    public Main(IServiceProvider sp) :
      this()
    {
      _repoMgrServer = sp.GetService<IRepositoryManagerServer>();
      _repoServer = sp.GetService<IRepositoryServer>();
      _graphServer = sp.GetService<IGraphServer>();
    }

    private void Overview_DoubleClick(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;

      if (selNode.Parent == null)
      {
        RefreshSystem(selNode);
      }

      if (selNode.Tag is RepositoryManager repoMgr)
      {
        RefreshRepositoryManager(selNode);
      }

      if (selNode.Tag is Repository repo)
      {
        RefreshRepository(selNode);
      }
    }

    private void RefreshSystem(TreeNode selNode)
    {
      using (new AutoCursor())
      {
        selNode.Nodes.Clear();
        var repoMgrs = _repoMgrServer.GetAll();
        var childNodes = repoMgrs.Select(x => new TreeNode(x.Name) { Tag = x });
        selNode.Nodes.AddRange(childNodes.ToArray());
        selNode.ExpandAll();
      }
    }

    private void RefreshRepositoryManager(TreeNode selNode)
    {
      using (new AutoCursor())
      {
        selNode.Nodes.Clear();
        var repoMgr = (RepositoryManager)selNode.Tag;
        var repos = _repoServer.ByOwners(new[] { repoMgr.Id });
        var childNodes = repos.Select(x => new TreeNode(x.Name) { Tag = x });
        selNode.Nodes.AddRange(childNodes.ToArray());
        selNode.ExpandAll();
      }
    }

    private void RefreshRepository(TreeNode selNode)
    {
      using (new AutoCursor())
      {
        selNode.Nodes.Clear();
        var repo = (Repository)selNode.Tag;
        var repos = _graphServer.ByOwners(new[] { repo.Id });
        var childNodes = repos.Select(x => new TreeNode(x.Name) { Tag = x });
        selNode.Nodes.AddRange(childNodes.ToArray());
        selNode.ExpandAll();
      }
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
      if (selNode.Tag is RepositoryManager)
      {
        RepositoryManagerMenu.Show(Overview, new Point(e.X, e.Y));
      }

      if (selNode.Tag is Repository)
      {
        RepositoryMenu.Show(Overview, new Point(e.X, e.Y));
      }
    }

    private void CmdImport_Click(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var repo = (Repository)selNode.Tag;
      var import = new Import(repo);
      import.ShowDialog();
    }

    private void CmdCreate_Click(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var repoMgr = (RepositoryManager)selNode.Tag;
      var repo = new Repository(repoMgr.Id, "New repository");
      var createEdit = new CreateEdit(repo);
      if (createEdit.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      using (new AutoCursor())
      {
        _repoServer.Create(new[] { repo });
        RefreshRepositoryManager(selNode);
      }
    }

    private void CmdEdit_Click(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var repo = (Repository)selNode.Tag;
      var createEdit = new CreateEdit(repo);
      if (createEdit.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      using (new AutoCursor())
      {
        _repoServer.Update(new[] { repo });
        RefreshRepositoryManager(selNode.Parent);
      }
    }

    private void CmdDelete_Click(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var repo = (Repository)selNode.Tag;
      if (MessageBox.Show($"Are you sure you want to delete repository:  {repo.Name}", "Confirm delete repository", MessageBoxButtons.OKCancel) != DialogResult.OK)
      {
        return;
      }

      using (new AutoCursor())
      {
        _repoServer.Delete(new[] { repo });
        RefreshRepositoryManager(selNode.Parent);
      }
    }
  }
}
