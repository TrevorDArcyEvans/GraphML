using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using BlazorContextMenu;
using GraphML.Interfaces.Server;
using GraphML.UI.Web.Models;
using GraphML.UI.Web.Widgets;
using GraphShape.Algorithms.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using QG = QuikGraph;
using Point = Blazor.Diagrams.Core.Geometry.Point;

namespace GraphML.UI.Web.Pages
{
  public partial class ShowChart
  {
    #region Parameters

    [Parameter]
    public string OrganisationName { get; set; }

    [Parameter]
    public string OrganisationId { get; set; }

    [Parameter]
    public string RepositoryManagerName { get; set; }

    [Parameter]
    public string RepositoryManagerId { get; set; }

    [Parameter]
    public string RepositoryName { get; set; }

    [Parameter]
    public string RepositoryId { get; set; }

    [Parameter]
    public string GraphName { get; set; }

    [Parameter]
    public string GraphId { get; set; }

    [Parameter]
    public string ChartName { get; set; }

    [Parameter]
    public string ChartId { get; set; }

    #endregion

    #region Inject

    [Inject]
    private IBlazorContextMenuService _contextMenuService { get; set; }

    [Inject]
    private INodeServer _nodeServer { get; set; }

    [Inject]
    private IEdgeServer _edgeServer { get; set; }

    [Inject]
    private IGraphServer _graphServer { get; set; }

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IGraphEdgeServer _graphEdgeServer { get; set; }

    [Inject]
    private IChartServer _chartServer { get; set; }

    [Inject]
    private IChartNodeServer _chartNodeServer { get; set; }

    [Inject]
    private IChartEdgeServer _chartEdgeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private Diagram _diagram { get; set; }
    private GraphNode[] _graphNodes;
    private Graph _graph;

    // GraphNode.Id
    private Guid _draggedNodeId;

    private bool _isNewNode;
    private bool _newDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;
    private Point _newNodePos;

    private bool _parentChildDialogIsOpen;
    private List<Node> _parentNodes = new List<Node>();
    private Node _selectedNode;
    private Node _childNode;

    private string _layout;

    protected override async void OnInitialized()
    {
      base.OnInitialized();

      var options = new DiagramOptions
      {
        DeleteKey = "Delete", // What key deletes the selected nodes/links
        DefaultNodeComponent = null, // Default component for nodes
        AllowMultiSelection = true, // Whether to allow multi selection using CTRL
        Links = new DiagramLinkOptions
        {
        },
        Zoom = new DiagramZoomOptions
        {
          Minimum = 0.5, // Minimum zoom value
          Inverse = false, // Whether to inverse the direction of the zoom when using the wheel
        }
      };
      _diagram = new Diagram(options);
      _diagram.MouseClick += Diagram_OnMouseClick;

      _diagram.RegisterModelComponent<DiagramNode, DiagramNodeWidget>();
      _diagram.RegisterModelComponent<LinkLabelModel, DiagramLinkLabelWidget>();

      var graphs = await _graphServer.ByIds(new[] { Guid.Parse(GraphId) });
      _graph = graphs.Single();
      var chartNodesPage = await _chartNodeServer.ByOwner(Guid.Parse(ChartId), 0, int.MaxValue, null);
      var chartEdgesPage = await _chartEdgeServer.ByOwner(Guid.Parse(ChartId), 0, int.MaxValue, null);
      var chartNodes = chartNodesPage.Items;
      var chartEdges = chartEdgesPage.Items;
      Setup(chartNodes, chartEdges);
    }

    private void Diagram_OnMouseClick(Model model, MouseEventArgs eventArgs)
    {
      if (eventArgs.Button == 2 &&
          model is DiagramNode)
      {
        _contextMenuService.ShowMenu("NodeContextMenu", (int) eventArgs.ClientX, (int) eventArgs.ClientY);
      }
    }

    private void Setup(IEnumerable<ChartNode> chartNodes, IEnumerable<ChartEdge> chartEdges)
    {
      var nodes = chartNodes.Select(chartNode =>
        new DiagramNode(chartNode, new Point(chartNode.X, chartNode.Y)));
      _diagram.Nodes.Add(nodes);

      var links = chartEdges.Select(chartEdge =>
      {
        var source = _diagram.Nodes.Single(n => n.Id == chartEdge.ChartSourceId.ToString());
        var target = _diagram.Nodes.Single(n => n.Id == chartEdge.ChartTargetId.ToString());
        var link = new DiagramLink(chartEdge, source, target)
        {
          TargetMarker = _graph.Directed ? LinkMarker.Arrow : null
        };
        return link;
      });
      _diagram.Links.Add(links);
    }

    private void OnDragNodeStart(Guid draggedNode)
    {
      // Can also use transferData, but this is probably "faster"
      _draggedNodeId = draggedNode;
    }

    private void OnDragNewStart()
    {
      _isNewNode = true;
    }

    private async Task OnDrop(DragEventArgs e)
    {
      if (_draggedNodeId != Guid.Empty)
      {
        await OnDropNode(e);
        return;
      }

      if (_isNewNode)
      {
        await OnDropNew(e);
        return;
      }

      // nothing selected
    }

    private async Task OnDropNode(DragEventArgs e)
    {
      if (_diagram.Nodes.OfType<DiagramNode>().Any(n => Guid.Parse(n.Id) == _draggedNodeId))
      {
        // node already on chart
        return;
      }

      // GraphNode --> ChartNode --> DiagramNode
      var draggedNodes = await _chartNodeServer.ByGraphItems(Guid.Parse(ChartId), new[] { _draggedNodeId });
      var draggedNode = draggedNodes.SingleOrDefault();
      if (draggedNode is null)
      {
        // GraphNode is not in this Chart, so create a ChartNode in this Chart
        var graphNodes = await _graphNodeServer.ByIds(new[] { _draggedNodeId });
        var graphNode = graphNodes.Single();
        draggedNode = new ChartNode(Guid.Parse(ChartId), Guid.Parse(OrganisationId), _draggedNodeId, graphNode.Name);
        var newNodes = await _chartNodeServer.Create(new[] { draggedNode });
      }

      var position = _diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
      var node = new DiagramNode(draggedNode, position);
      _diagram.Nodes.Add(node);

      _draggedNodeId = Guid.Empty;
    }

    private async Task OnDropNew(DragEventArgs e)
    {
      _isNewNode = false;

      // save position
      _newNodePos = _diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);

      // show dialog for new node name
      _dlgNewItemName = null;
      _newDialogIsOpen = true;
    }

    private async Task OkClick()
    {
      try
      {
        if (string.IsNullOrWhiteSpace(_dlgNewItemName))
        {
          return;
        }

        _newItemName = _dlgNewItemName;

        var orgId = Guid.Parse(OrganisationId);
        var repoId = Guid.Parse(RepositoryId);
        var graphId = Guid.Parse(GraphId);
        var chartId = Guid.Parse(ChartId);
        var node = new Node(repoId, orgId, _newItemName);
        var newNodes = await _nodeServer.Create(new[] { node });
        var graphNode = new GraphNode(graphId, orgId, node.Id, _newItemName);
        var newGraphNodes = await _graphNodeServer.Create(new[] { graphNode });
        var chartNode = new ChartNode(chartId, orgId, graphNode.Id, _newItemName);
        var newChartNodes = await _chartNodeServer.Create(new[] { chartNode });
        var diagNode = new DiagramNode(chartNode, _newNodePos);

        _diagram.Nodes.Add(diagNode);
      }
      finally
      {
        _newItemName = _dlgNewItemName = null;
        _newDialogIsOpen = false;
        _newNodePos = null;
      }
    }

    private async Task OnExpandNode(ItemClickEventArgs e)
    {
      // expand selected ChartNode to get GraphEdges
      var selChartNode = _diagram.GetSelectedModels().OfType<DiagramNode>().ToList().Single();
      var selGraphNodeId = selChartNode.ChartNode.GraphItemId;
      var selGraphNodes = await _graphNodeServer.ByIds(new[] { selGraphNodeId });
      var selGraphNode = selGraphNodes.Single();
      var expGraphEdgesPage = await _graphEdgeServer.ByNodeIds(new[] { selGraphNode.Id }, 0, int.MaxValue, null);
      var expGraphEdges = expGraphEdgesPage.Items;
      var expGraphEdgeIds = expGraphEdges.Select(ge => ge.Id);
      var expGraphNodeIds = expGraphEdges.SelectMany(ge => new[] { ge.GraphSourceId, ge.GraphTargetId }).Distinct();


      // work out what GraphNodes we already have in Diagram
      var chartNodes = _diagram.Nodes.OfType<DiagramNode>().Select(diagNode => diagNode.ChartNode);
      var graphNodeIds = chartNodes.Select(cn => cn.GraphItemId);

      // work out missing GraphNodes = already in Diagram but not in expansion
      var missGraphNodeIds = expGraphNodeIds.Except(graphNodeIds).ToList();
      var missChartNodes = (await _chartNodeServer.ByGraphItems(Guid.Parse(ChartId), missGraphNodeIds)).ToList();
      if (missChartNodes.Count() != missGraphNodeIds.Count())
      {
        // GraphNode.Id of ChartNodes which are in Repository
        var repoChartGraphNodeIds = missChartNodes.Select(cn => cn.GraphItemId);

        // get GraphNode.Id of ChartNodes which are not in Repository
        var missRepoChartGraphNodeIds = missGraphNodeIds.Except(repoChartGraphNodeIds);

        // create a ChartNode in Repository for each missing GraphNode
        var missRepoChartGraphNodes = await _graphNodeServer.ByIds(missRepoChartGraphNodeIds);
        var missRepoChartNodes = missRepoChartGraphNodes.Select(gn =>
          new ChartNode(Guid.Parse(ChartId), Guid.Parse(OrganisationId), gn.Id, gn.Name)).ToList();
        await _chartNodeServer.Create(missRepoChartNodes);
        missChartNodes.AddRange(missRepoChartNodes);
      }


      // work out what GraphEdges we already have in Diagram
      var chartEdges = _diagram.Links.OfType<DiagramLink>().Select(diagLink => diagLink.ChartEdge);
      var graphEdgeIds = chartEdges.Select(ce => ce.GraphItemId);

      // work out missing GraphEdges = already in Diagram but not in expansion
      var missGraphEdgeIds = expGraphEdgeIds.Except(graphEdgeIds).ToList();
      var missChartEdges = (await _chartEdgeServer.ByGraphItems(Guid.Parse(ChartId), missGraphEdgeIds)).ToList();
      if (missChartEdges.Count() != missGraphEdgeIds.Count())
      {
        // GraphEdge.Id of ChartEdges which are in Repository
        var repoChartGraphEdgeIds = missChartEdges.Select(ce => ce.GraphItemId);

        // get GraphEdge.Id of ChartEdges which are not in Repository
        var missRepoChartGraphEdgeIds = missGraphEdgeIds.Except(repoChartGraphEdgeIds);

        // create a ChartEdge in Repository for each missing GraphEdge
        var missRepoChartGraphEdges = await _graphEdgeServer.ByIds(missRepoChartGraphEdgeIds);
        var missRepoChartEdges = missRepoChartGraphEdges.Select(async ge =>
          {
            var chartNodeSources = await _chartNodeServer.ByGraphItems(Guid.Parse(ChartId), new[] { ge.GraphSourceId });
            var chartNodeSource = chartNodeSources.Single();
            var chartNodeTargets = await _chartNodeServer.ByGraphItems(Guid.Parse(ChartId), new[] { ge.GraphTargetId });
            var chartNodeTarget = chartNodeTargets.Single();
            return new ChartEdge(Guid.Parse(ChartId), Guid.Parse(OrganisationId), ge.Id, ge.Name, chartNodeSource.Id, chartNodeTarget.Id);
          })
          .Select(t => t.Result)
          .ToList();
        await _chartEdgeServer.Create(missRepoChartEdges);
        missChartEdges.AddRange(missRepoChartEdges);
      }

      // create missing nodes
      var nodes = missChartNodes.Select(chartNode =>
        new DiagramNode(chartNode, new Point(chartNode.X, chartNode.Y)));
      _diagram.Nodes.Add(nodes);


      // create missing edges
      var links = missChartEdges.Select(chartEdge =>
      {
        var source = _diagram.Nodes.Single(n => n.Id == chartEdge.ChartSourceId.ToString());
        var target = _diagram.Nodes.Single(n => n.Id == chartEdge.ChartTargetId.ToString());
        var link = new DiagramLink(chartEdge, source, target)
        {
          TargetMarker = _graph.Directed ? LinkMarker.Arrow : null
        };
        return link;
      });
      _diagram.Links.Add(links);
    }

    private async Task OnShowParentChild(ItemClickEventArgs e)
    {
      var selChartNode = _diagram.GetSelectedModels().OfType<DiagramNode>().Single();
      var selGraphNodeId = selChartNode.ChartNode.GraphItemId;
      var selGraphNodes = await _graphNodeServer.ByIds(new[] { selGraphNodeId });
      var selGraphNode = selGraphNodes.Single();
      var parentsPage = await _nodeServer.GetParents(selGraphNode.RepositoryItemId, 0, int.MaxValue, null);
      _parentNodes = parentsPage.Items;
      var thisNodePage = await _nodeServer.ByIds(new[] { selGraphNode.RepositoryItemId });
      _selectedNode = thisNodePage.Single();
      var children = await _nodeServer.ByIds(new[] { _selectedNode?.NextId ?? Guid.Empty });
      _childNode = children.SingleOrDefault();
      _parentChildDialogIsOpen = true;
    }

    private async Task OnSave()
    {
      // get all ChartNodes from Repository
      var allChartNodesPage = await _chartNodeServer.ByOwner(Guid.Parse(ChartId), 0, int.MaxValue, null);
      var allChartNodes = allChartNodesPage.Items;
      var allChartNodeIds = allChartNodes.Select(cn => cn.Id);

      // get all ChartNodes in this Diagram
      var chartNodes = _diagram.Nodes.OfType<DiagramNode>().Select(dn =>
      {
        dn.ChartNode.X = (int) dn.Position.X;
        dn.ChartNode.Y = (int) dn.Position.Y;
        return dn.ChartNode;
      }).ToList();
      var chartNodeIds = chartNodes.Select(cn => cn.Id);

      // work out which ChartNodes are in Repository but not in Diagram ie deleted from Diagram
      var missNodeIds = allChartNodeIds.Except(chartNodeIds).ToList();
      var missNodes = allChartNodes.Where(cn => missNodeIds.Contains(cn.Id));

      // delete missing ChartNodes from Repository
      await _chartNodeServer.Delete(missNodes);


      // get all ChartEdges from Repository
      var allChartEdgesPage = await _chartEdgeServer.ByOwner(Guid.Parse(ChartId), 0, int.MaxValue, null);
      var allChartEdges = allChartEdgesPage.Items;
      var allChartEdgeNodeIds = allChartEdges.Select(ce => ce.Id);

      // get all ChartEdges in this Diagram
      var chartEdges = _diagram.Links.OfType<DiagramLink>().Select(dl => dl.ChartEdge);
      var chartEdgeIds = chartEdges.Select(ce => ce.Id);

      // work out which ChartEdges are in Repository but not in Diagram ie delete from Diagram
      var missEdgeIds = allChartEdgeNodeIds.Except(chartEdgeIds);
      var missEdges = allChartEdges.Where(ce => missEdgeIds.Contains(ce.Id));

      // delete missing ChartEdges from Repository
      await _chartEdgeServer.Delete(missEdges);


      // delete dangling DiagramLinks
      // BUG:   portless charts do not seem to delete links when an attached node is deleted
      //        Further, the attached node does not appear to be removed from the link!
      var dangChartEdges = _diagram.Links.OfType<DiagramLink>()
        .Where(dl => missNodeIds.Contains(dl.ChartEdge.ChartSourceId) || missNodeIds.Contains(dl.ChartEdge.ChartTargetId))
        .Select(dl => dl.ChartEdge);
      await _chartEdgeServer.Delete(dangChartEdges);

      // only save ChartNodes in Diagram
      await _chartNodeServer.Update(chartNodes);
    }

    private void OnLayout(string layout)
    {
      if (string.IsNullOrWhiteSpace(layout))
      {
        return;
      }

      var graph = new QG.BidirectionalGraph<DiagramNode, QG.Edge<DiagramNode>>();
      var nodes = _diagram.Nodes.OfType<DiagramNode>();
      var edges = _diagram.Links.OfType<DiagramLink>()
        .Select(dl =>
        {
          var source = nodes.Single(dn => Guid.Parse(dn.Id) == dl.ChartEdge.ChartSourceId);
          var target = nodes.Single(dn => Guid.Parse(dn.Id) == dl.ChartEdge.ChartSourceId);
          return new QG.Edge<DiagramNode>(source, target);
        })
        .ToList();
      graph.AddVertexRange(nodes);
      graph.AddEdgeRange(edges);

      var positions = nodes.ToDictionary(dn => dn, dn => new GraphShape.Point(dn.Position.X, dn.Position.Y));
      var sizes = nodes.ToDictionary(dn => dn, dn => new GraphShape.Size(dn.Size?.Width ?? 100, dn.Size?.Height ?? 100));
      var layoutCtx = new LayoutContext<DiagramNode, QG.Edge<DiagramNode>, QG.BidirectionalGraph<DiagramNode, QG.Edge<DiagramNode>>>(graph, positions, sizes, LayoutMode.Simple);
      var algoFact = new StandardLayoutAlgorithmFactory<DiagramNode, QG.Edge<DiagramNode>, QG.BidirectionalGraph<DiagramNode, QG.Edge<DiagramNode>>>();
      var algo = algoFact.CreateAlgorithm(layout, layoutCtx, null);

      algo.Compute();

      try
      {
        _diagram.SuspendRefresh = true;
        foreach (var vertPos in algo.VerticesPositions)
        {
          vertPos.Key.Position = new Point(vertPos.Value.X, vertPos.Value.Y);

          // BUG:   Diagram.Refresh does not redraw node until node is selected
          //        so select all nodes and then unselect them
          _diagram.SelectModel(vertPos.Key, false);
        }
      }
      finally
      {
        _diagram.SuspendRefresh = false;
      }

      _diagram.Refresh();
      _diagram.UnselectAll();
    }

    private void GotoBrowseCharts()
    {
      _navMgr.NavigateTo($"/BrowseCharts/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
