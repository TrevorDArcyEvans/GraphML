using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using BlazorContextMenu;
using GraphML.UI.Web.Models;
using GraphML.UI.Web.Widgets;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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

    private Diagram _diagram { get; set; }
    private GraphNode[] _graphNodes;
    private Graph _graph;

    // GraphNode.Id
    private Guid _draggedNodeId;

    private bool _parentChildDialogIsOpen;
    private List<Node> _parentNodes = new List<Node>();
    private Node _selectedNode;
    private Node _childNode;

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
          model is DiagramNode itemNode)
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

    private void OnDragStart(Guid draggedNode)
    {
      // Can also use transferData, but this is probably "faster"
      _draggedNodeId = draggedNode;
    }

    private async Task OnDrop(DragEventArgs e)
    {
      if (_draggedNodeId == Guid.Empty)
      {
        // nothing selected
        return;
      }

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
      var missGraphNodeIds = expGraphNodeIds.Except(graphNodeIds);
      var missChartNodes = await _chartNodeServer.ByGraphItems(Guid.Parse(ChartId), missGraphNodeIds);


      // work out what GraphEdges we already have in Diagram
      var chartEdges = _diagram.Links.OfType<DiagramLink>().Select(diagLink => diagLink.ChartEdge);
      var graphEdgeIds = chartEdges.Select(ce => ce.GraphItemId);

      // work out missing GraphEdges = already in Diagram but not in expansion
      var missGraphEdgeIds = expGraphEdgeIds.Except(graphEdgeIds);
      var missChartEdges = await _chartEdgeServer.ByGraphItems(Guid.Parse(ChartId), missGraphEdgeIds);


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
      var children = await _nodeServer.ByIds(new[] { _selectedNode.NextId });
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
      var chartNodes = _diagram.Nodes.OfType<DiagramNode>().Select(inode =>
      {
        inode.ChartNode.X = (int) inode.Position.X;
        inode.ChartNode.Y = (int) inode.Position.Y;
        return inode.ChartNode;
      });
      var chartNodeIds = chartNodes.Select(cn => cn.Id);
      
      // work out which ChartNodes are in Repository but not in Diagram ie deleted from Diagram
      var missNodeIds = allChartNodeIds.Except(chartNodeIds);
      var missNodes = allChartNodes.Where(cn => missNodeIds.Contains(cn.Id));

      // delete missing nodes
      await _chartNodeServer.Delete(missNodes);
      
      // only save ChartNodes in Diagram
      await _chartNodeServer.Update(chartNodes);
    }

    private void GotoBrowseCharts()
    {
      _navMgr.NavigateTo($"/BrowseCharts/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
