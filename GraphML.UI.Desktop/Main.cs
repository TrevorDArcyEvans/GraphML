using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
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
        }

        if (selNode.Tag is RepositoryManager repoMgr)
        {
          selNode.Nodes.Clear();
          var repos = _repoServer.ByOwners(new[] { repoMgr.Id });
          var childNodes = repos.Select(x => new TreeNode(x.Name) { Tag = x });
          selNode.Nodes.AddRange(childNodes.ToArray());
        }

        if (selNode.Tag is Repository repo)
        {
          // TODO   expand nodes+edges?
        }
      }
    }
  }
}
