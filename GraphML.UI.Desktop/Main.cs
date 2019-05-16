using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using GraphML.API.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphML.UI.Desktop
{
  public partial class Main : Form
  {
    private readonly IRepositoryManagerServer _repoMgrServer;
    private readonly IRepositoryServer _repoServer;
    private readonly IGraphServer _graphServer;
    private readonly INodeServer _nodeServer;
    private readonly IEdgeServer _edgeServer;
    private readonly IContactServer _contactServer;
    private readonly IOrganisationServer _organisationServer;
    private readonly IGraphItemAttributeServer _graphItemAttributeServer;
    private readonly IEdgeItemAttributeServer _edgeItemAttributeServer;
    private readonly INodeItemAttributeServer _nodeItemAttributeServer;
    private readonly IRepositoryItemAttributeServer _repositoryItemAttributeServer;
    private readonly IAnalysisServer _analysisServer;
    private readonly IResultServer _resultServer;

    public Main()
    {
      InitializeComponent();
    }

    public Main(
      IRepositoryManagerServer repoMgrServer,
      IRepositoryServer repoServer,
      IGraphServer graphServer,
      INodeServer nodeServer,
      IEdgeServer edgeServer,
      IContactServer contactServer,
      IOrganisationServer organisationServer,
      IGraphItemAttributeServer graphItemAttributeServer,
      IEdgeItemAttributeServer edgeItemAttributeServer,
      INodeItemAttributeServer nodeItemAttributeServer,
      IRepositoryItemAttributeServer repositoryItemAttributeServer,
      IAnalysisServer analysisServer,
      IResultServer resultServer) :
      this()
    {
      _repoMgrServer = repoMgrServer;
      _repoServer = repoServer;
      _graphServer = graphServer;
      _nodeServer = nodeServer;
      _edgeServer = edgeServer;
      _contactServer = contactServer;
      _organisationServer = organisationServer;
      _graphItemAttributeServer = graphItemAttributeServer;
      _edgeItemAttributeServer = edgeItemAttributeServer;
      _nodeItemAttributeServer = nodeItemAttributeServer;
      _repositoryItemAttributeServer = repositoryItemAttributeServer;
      _analysisServer = analysisServer;
      _resultServer = resultServer;

      //var req1 = new BetweennessRequest();
      //var req2 = new ClosenessRequest();
      //var req3 = new DegreeRequest();
      //var req4 = new FindShortestPathsRequest();
      //
      //var res1 = new BetweennessResult();
      //var res2 = new ClosenessResult();
      //var res3 = new DegreeResult();
      //var res4 = new FindShortestPathsResult();
    }

    private async void Overview_DoubleClickAsync(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;

      if (selNode.Parent == null)
      {
        RefreshSystem(selNode);
      }

      if (selNode.Tag is Organisation)
      {
        RefreshRepositoryManager(selNode);
      }

      if (selNode.Tag is RepositoryManager)
      {
        RefreshRepositoryAsync(selNode);
      }

      if (selNode.Tag is Repository)
      {
        RefreshGraphAsync(selNode);
      }
    }

    private async void RefreshSystem(TreeNode selNode)
    {
      using (new AutoCursor())
      {
        selNode.Nodes.Clear();
        var orgs = await _organisationServer.GetAll();
        var childNodes = orgs.Select(x => new TreeNode(x.Name) { Tag = x });
        selNode.Nodes.AddRange(childNodes.ToArray());
        selNode.ExpandAll();
      }
    }

    private async void RefreshRepositoryManager(TreeNode selNode)
    {
      using (new AutoCursor())
      {
        selNode.Nodes.Clear();
        var org = (Organisation)selNode.Tag;
        var repoMgrs = await _repoMgrServer.ByOwners(new[] { org.Id });
        var childNodes = repoMgrs.Select(x => new TreeNode(x.Name) { Tag = x });
        selNode.Nodes.AddRange(childNodes.ToArray());
        selNode.ExpandAll();
      }
    }

    private async void RefreshRepositoryAsync(TreeNode selNode)
    {
      using (new AutoCursor())
      {
        selNode.Nodes.Clear();
        var repoMgr = (RepositoryManager)selNode.Tag;
        var repos = await _repoServer.ByOwners(new[] { repoMgr.Id });
        var childNodes = repos.Select(x => new TreeNode(x.Name) { Tag = x });
        selNode.Nodes.AddRange(childNodes.ToArray());
        selNode.ExpandAll();
      }
    }

    private async void RefreshGraphAsync(TreeNode selNode)
    {
      using (new AutoCursor())
      {
        selNode.Nodes.Clear();
        var repo = (Repository)selNode.Tag;
        var graphs = await _graphServer.ByOwners(new[] { repo.Id });
        var childNodes = graphs.Select(x => new TreeNode(x.Name) { Tag = x });
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

      if (selNode.Tag is Graph)
      {
        GraphMenu.Show(Overview, new Point(e.X, e.Y));
      }
    }

    private void CmdRepositoryImport_Click(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var repo = (Repository)selNode.Tag;
      var import = new Import(repo);
      import.ShowDialog();
    }

    private async void CmdRepositoryCreate_ClickAsync(object sender, EventArgs e)
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
        await _repoServer.Create(new[] { repo });
        RefreshRepositoryManager(selNode);
      }
    }

    private async void CmdRepositoryEdit_ClickAsync(object sender, EventArgs e)
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
        await _repoServer.Update(new[] { repo });
        RefreshRepositoryManager(selNode.Parent);
      }
    }

    private async void CmdRepositoryDelete_ClickAsync(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var repo = (Repository)selNode.Tag;
      if (MessageBox.Show($"Are you sure you want to delete repository:  {repo.Name}", "Confirm delete repository", MessageBoxButtons.OKCancel) != DialogResult.OK)
      {
        return;
      }

      using (new AutoCursor())
      {
        await _repoServer.Delete(new[] { repo });
        RefreshRepositoryManager(selNode.Parent);
      }
    }

    private async void CmdGraphDelete_ClickAsync(object sender, EventArgs e)
    {
      var selNode = Overview.SelectedNode;
      var graph = (Graph)selNode.Tag;
      if (MessageBox.Show($"Are you sure you want to delete graph:  {graph.Name}", "Confirm delete graph", MessageBoxButtons.OKCancel) != DialogResult.OK)
      {
        return;
      }

      using (new AutoCursor())
      {
        await _graphServer.Delete(new[] { graph });
        RefreshRepositoryAsync(selNode.Parent);
      }
    }
  }
}
