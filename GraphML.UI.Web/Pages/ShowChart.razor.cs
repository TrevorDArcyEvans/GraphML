using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using BlazorContextMenu;
using GraphML.Porcelain;
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
    private ChartEx _chart;
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

      _diagram.RegisterModelComponent<ItemNode, ItemNodeWidget>();

      _chart = await _chartExServer.ById(Guid.Parse(ChartId));
      var chartNodesPage = await _chartNodeServer.ByOwner(Guid.Parse(ChartId), 0, int.MaxValue, null);
      var chartEdgesPage = await _chartEdgeServer.ByOwner(Guid.Parse(ChartId), 0, int.MaxValue, null);
      var chartNodes = chartNodesPage.Items;
      var chartEdges = chartEdgesPage.Items;
      Setup(chartNodes, _chart);
    }

    private void Diagram_OnMouseClick(Model model, MouseEventArgs eventArgs)
    {
      if (eventArgs.Button == 2 &&
          model is ItemNode itemNode)
      {
        _contextMenuService.ShowMenu("NodeContextMenu", (int) eventArgs.ClientX, (int) eventArgs.ClientY);
      }
    }

    private void Setup(IEnumerable<ChartNode> chartNodes, ChartEx chart)
    {
      var nodes = chartNodes.Select(n =>
        new ItemNode(n, new Point(n.X, n.Y)));
      _diagram.Nodes.Add(nodes);

      // var links = chart.Edges.Select(edge =>
      // {
      //   var source = _diagram.Nodes.Single(n => n.Id == edge.SourceId.ToString());
      //   var target = _diagram.Nodes.Single(n => n.Id == edge.TargetId.ToString());
      //   var link = new LinkModel(edge.RepositoryItemId.ToString(), source, target) // TODO  store ChartEdge
      //   {
      //     TargetMarker = _chart.Directed ? LinkMarker.Arrow : null
      //   };
      //   link.Labels.Add(new LinkLabelModel(link, edge.Name));
      //   return link;
      // });
      // _diagram.Links.Add(links);
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

      if (_diagram.Nodes.OfType<ItemNode>().Any(n => Guid.Parse(n.Id) == _draggedNodeId))
      {
        // node already on chart
        return;
      }

      // TODO   Node --> GraphNode --> ChartNode --> DiagramNode
      var draggedNodePage = await _chartNodeServer.ByOwner(_draggedNodeId, 0, int.MaxValue, null);
      var draggedNode = draggedNodePage.Items.SingleOrDefault(n => n.ChartId == Guid.Parse(ChartId));
      var position = _diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
      var node = new ItemNode(draggedNode, position); // TODO  store ChartNode
      _diagram.Nodes.Add(node);

      _draggedNodeId = Guid.Empty;
    }

    private async Task OnExpandNode(ItemClickEventArgs e)
    {
      // expand node
      var selNode = _diagram.GetSelectedModels().OfType<ItemNode>().Single();
      var expPageEdges = await _edgeServer.ByNodeIds(new[] { Guid.Parse(selNode.Id) }, 0, int.MaxValue, null);
      var expEdges = expPageEdges.Items;

      // work out missing edges+nodes
      var expEdgeIds = expEdges.Select(edge => edge.Id.ToString());
      var chartEdgeIds = _diagram.Links.Select(link => link.Id);
      var missEdgeIds = expEdgeIds.Except(chartEdgeIds);
      var missEdges = expEdges.Where(expEdge => missEdgeIds.Contains(expEdge.Id.ToString()));
      var missEdgeNodeIds = missEdges.SelectMany(edge => new[] { edge.SourceId.ToString(), edge.TargetId.ToString() }).Distinct();
      var chartNodeIds = _diagram.Nodes.Select(node => node.Id);
      var missNodeIds = missEdgeNodeIds.Except(chartNodeIds);

      // create missing nodes
      // TODO   Node --> GraphNode --> ChartNode --> DiagramNode
      var missNodeGuids = missNodeIds.Select(id => Guid.Parse(id));
      var missNodes = await _nodeServer.ByIds(missNodeGuids);
      //var nodes = missNodes.Select(n => new ItemNode(n.Id.ToString(), n.Name, new Point(10, 10))); // TODO  store ChartNode
      //_diagram.Nodes.Add(nodes);

      // create missing edges
      // TODO   Edge --> GraphEdge --> ChartEdge --> DiagramEdge
      var links = missEdges.Select(edge =>
      {
        var source = _diagram.Nodes.Single(n => n.Id == edge.SourceId.ToString());
        var target = _diagram.Nodes.Single(n => n.Id == edge.TargetId.ToString());
        var link = new LinkModel(edge.Id.ToString(), source, target) // TODO  store ChartEdge
        {
          TargetMarker = _chart.Directed ? LinkMarker.Arrow : null
        };
        link.Labels.Add(new LinkLabelModel(link, edge.Name));
        return link;
      });
      _diagram.Links.Add(links);
    }

    private async Task OnShowParentChild(ItemClickEventArgs e)
    {
      var selNode = _diagram.GetSelectedModels().OfType<ItemNode>().Single();
      var selNodeId = Guid.Parse(selNode.Id);
      var parentsPage = await _nodeServer.GetParents(selNodeId, 0, int.MaxValue, null);
      _parentNodes = parentsPage.Items;
      var thisNodePage = await _nodeServer.ByIds(new[] { selNodeId });
       _selectedNode = thisNodePage.Single();
      var children = await _nodeServer.ByIds(new[] { _selectedNode.NextId });
       _childNode = children.SingleOrDefault();
      _parentChildDialogIsOpen = true;
    }

    private void OnSave()
    {
      // TODO   OnSave
    }

    private void GotoBrowseCharts()
    {
      _navMgr.NavigateTo($"/BrowseCharts/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
