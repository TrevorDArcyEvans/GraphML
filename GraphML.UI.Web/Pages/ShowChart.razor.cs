using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using BlazorContextMenu;
using GraphML.Interfaces.Server;
using GraphML.UI.Web.Models;
using GraphML.UI.Web.Widgets;
using GraphML.Utils;
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

    #endregion

    private const int ChunkSize = 1000;
    private const int DegreeOfParallelism = 10;

    private static readonly List<string> ImageExtensions = new() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

    #region member variables

    private readonly Diagram _diagram = new(GetOptions());
    private GraphNode[] _graphNodes;
    private List<DiagramNode> _diagNodes;
    private Graph _graph;

    private Guid _orgId;
    private Guid _graphId;
    private Guid _chartId;

    // GraphNode.Id
    private Guid _draggedNodeId;

    private IconInfo[] _icons;

    private bool _isNewNode;
    private string _newIconName;
    private bool _newDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;
    private Point _newNodePos;

    private bool _parentChildDialogIsOpen;
    private List<Node> _parentNodes = new();
    private Node _selectedNode;
    private Node _childNode;

    private bool _editNodeDialogIsOpen;
    private string _editNodeName;
    private string _dlgEditNodeName;

    private string _dlgEditNodeIconName;

    private bool _editLinkDialogIsOpen;
    private string _editLinkName;
    private string _dlgEditLinkName;

    private string _layout;

    private bool _isBusy;

    #endregion

    #region Initialisation

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      _orgId = Guid.Parse(OrganisationId);
      _graphId = Guid.Parse(GraphId);
      _chartId = Guid.Parse(ChartId);

      _diagram.MouseClick += Diagram_OnMouseClick;

      _diagram.RegisterModelComponent<DiagramNode, DiagramNodeWidget>();
      _diagram.RegisterModelComponent<DiagramLinkLabel, DiagramLinkLabelWidget>();

      var iconDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "icons");
      _icons = Directory
        .GetFiles(iconDir, "*", SearchOption.AllDirectories)
        .Where(fileName => ImageExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant()))
        .Select(fileName => Path.GetRelativePath("wwwroot", fileName))
        .Select(path => new IconInfo(path))
        .ToArray();

      var graphs = await _graphServer.ByIds(new[] { _graphId });
      _graph = graphs.Single();
      var chartNodes = await GetChartNodes();
      var chartEdges = await GetChartEdges();
      await Setup(chartNodes, chartEdges);
    }

    private static DiagramOptions GetOptions()
    {
      return new DiagramOptions
      {
        DeleteKey = "Delete", // What key deletes the selected nodes/links
        DefaultNodeComponent = null, // Default component for nodes
        AllowMultiSelection = true, // Whether to allow multi selection using CTRL
        Links = new DiagramLinkOptions
        {
        },
        Zoom = new DiagramZoomOptions
        {
          Minimum = 0.1,
          Maximum = 4.0,
          Inverse = false, // Whether to inverse the direction of the zoom when using the wheel
        }
      };
    }

    private async Task<List<ChartNode>> GetChartNodes()
    {
      var numData = await _chartNodeServer.Count(_chartId);
      var numChunks = (numData / ChunkSize) + 1;
      var chunkRange = Enumerable.Range(0, numChunks);
      var retval = new ConcurrentBag<ChartNode>();
      await chunkRange.ParallelForEachAsync(DegreeOfParallelism, async i =>
      {
        var chartNodesPage = await _chartNodeServer.ByOwner(_chartId, i + 1, ChunkSize, null);
        var chartNodes = chartNodesPage.Items.ToList();
        chartNodes.ForEach(cn => retval.Add(cn));
      });

      return retval.ToList();
    }

    private async Task<List<ChartEdge>> GetChartEdges()
    {
      var numData = await _chartEdgeServer.Count(_chartId);
      var numChunks = (numData / ChunkSize) + 1;
      var chunkRange = Enumerable.Range(0, numChunks);
      var retval = new ConcurrentBag<ChartEdge>();
      await chunkRange.ParallelForEachAsync(DegreeOfParallelism, async i =>
      {
        var chartEdgesPage = await _chartEdgeServer.ByOwner(_chartId, i + 1, ChunkSize, null);
        var chartEdges = chartEdgesPage.Items.ToList();
        chartEdges.ForEach(cn => retval.Add(cn));
      });

      return retval.ToList();
    }

    private async Task Setup(IEnumerable<ChartNode> chartNodes, IEnumerable<ChartEdge> chartEdges)
    {
      try
      {
        _isBusy = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        _diagram.Nodes.Added += UpdateDiagramNodes();
        _diagram.Nodes.Removed += UpdateDiagramNodes();

        var nodes = chartNodes.Select(chartNode =>
            new DiagramNode(chartNode, new Point(chartNode.X, chartNode.Y))
            {
              IconName = chartNode.IconName
            })
          .ToList();
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
      finally
      {
        _isBusy = false;
      }
    }

    #endregion

    private void Diagram_OnMouseClick(Model model, MouseEventArgs eventArgs)
    {
      if (eventArgs.Button == 2 &&
          model is DiagramNode)
      {
        _contextMenuService.ShowMenu("NodeContextMenu", (int) eventArgs.ClientX, (int) eventArgs.ClientY);
      }

      if (eventArgs.Button == 2 &&
          model is DiagramLink)
      {
        _contextMenuService.ShowMenu("LinkContextMenu", (int) eventArgs.ClientX, (int) eventArgs.ClientY);
      }
    }

    #region Drag-Drop

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

    #region Drag-Drop existing node

    private void OnDragNodeStart(Guid draggedNode)
    {
      // Can also use transferData, but this is probably "faster"
      _draggedNodeId = draggedNode;
    }

    private async Task OnDropNode(DragEventArgs e)
    {
      try
      {
        if (_diagram.Nodes.OfType<DiagramNode>().Any(n => Guid.Parse(n.Id) == _draggedNodeId))
        {
          // node already on chart
          return;
        }

        // GraphNode --> ChartNode --> DiagramNode
        var draggedNodes = await _chartNodeServer.ByGraphItems(_chartId, new[] { _draggedNodeId });
        var draggedNode = draggedNodes.SingleOrDefault();
        if (draggedNode is null)
        {
          // GraphNode is not in this Chart, so create a ChartNode in this Chart
          var graphNodes = await _graphNodeServer.ByIds(new[] { _draggedNodeId });
          var graphNode = graphNodes.Single();
          draggedNode = new ChartNode(_chartId, _orgId, _draggedNodeId, graphNode.Name);
          var newNodes = await _chartNodeServer.Create(new[] { draggedNode });
        }

        var position = _diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
        var node = new DiagramNode(draggedNode, position)
        {
          IconName = draggedNode.IconName
        };
        _diagram.Nodes.Add(node);
      }
      finally
      {
        _draggedNodeId = Guid.Empty;
      }
    }

    #endregion

    #region Drag-Drop new node

    private void OnDragNewStart(string iconName)
    {
      _isNewNode = true;
      _newIconName = iconName;
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

    private async Task OkNewClick()
    {
      try
      {
        if (string.IsNullOrWhiteSpace(_dlgNewItemName))
        {
          return;
        }

        _newItemName = _dlgNewItemName;

        var repoId = Guid.Parse(RepositoryId);
        var node = new Node(repoId, _orgId, _newItemName);
        var newNodes = await _nodeServer.Create(new[] { node });
        var graphNode = new GraphNode(_graphId, _orgId, node.Id, _newItemName);
        var newGraphNodes = await _graphNodeServer.Create(new[] { graphNode });
        var chartNode = new ChartNode(_chartId, _orgId, graphNode.Id, _newItemName)
        {
          IconName = _newIconName.ToString()
        };
        var newChartNodes = await _chartNodeServer.Create(new[] { chartNode });
        var diagNode = new DiagramNode(chartNode, _newNodePos)
        {
          IconName = _newIconName
        };

        _diagram.Nodes.Add(diagNode);
      }
      finally
      {
        _newIconName = null;
        _newItemName = _dlgNewItemName = null;
        _newDialogIsOpen = false;
        _newNodePos = null;
      }
    }

    #endregion

    #endregion

    #region Edit node

    private void OnEditNode(ItemClickEventArgs e)
    {
      var selChartNode = _diagram.GetSelectedModels().OfType<DiagramNode>().ToList().Single();
      _dlgEditNodeName = selChartNode.Name;
      _editNodeDialogIsOpen = true;
    }

    private void OkEditNodeClick()
    {
      try
      {
        if (string.IsNullOrWhiteSpace(_dlgEditNodeName))
        {
          return;
        }

        _editNodeName = _dlgEditNodeName;

        var selChartNode = _diagram.GetSelectedModels().OfType<DiagramNode>().ToList().Single();
        selChartNode.Name = _editNodeName;
        selChartNode.IconName = string.IsNullOrEmpty(_dlgEditNodeIconName) ? null : _dlgEditNodeIconName;
        selChartNode.Refresh();
      }
      finally
      {
        _editNodeName = _dlgEditNodeName = _dlgEditNodeIconName = null;
        _editNodeDialogIsOpen = false;
      }
    }

    #endregion

    #region Edit link

    private void OnEditLink(ItemClickEventArgs e)
    {
      var selChartLink = _diagram.GetSelectedModels().OfType<DiagramLink>().ToList().Single();
      _dlgEditLinkName = selChartLink.Name;
      _editLinkDialogIsOpen = true;
    }

    private void OkEditLinkClick()
    {
      try
      {
        if (string.IsNullOrWhiteSpace(_dlgEditLinkName))
        {
          return;
        }

        _editLinkName = _dlgEditLinkName;

        var selChartLink = _diagram.GetSelectedModels().OfType<DiagramLink>().ToList().Single();
        selChartLink.Name = _editLinkName;
        selChartLink.Refresh();
      }
      finally
      {
        _editLinkName = _dlgEditLinkName = null;
        _editLinkDialogIsOpen = false;
      }
    }

    #endregion

    private async Task OnExpandNode(ItemClickEventArgs e)
    {
      try
      {
        _isBusy = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        // expand selected ChartNode to get GraphEdges
        var selChartNode = _diagram.GetSelectedModels().OfType<DiagramNode>().ToList().Single();
        var selGraphNodeId = selChartNode.ChartNode.GraphItemId;
        var selGraphNodes = await _graphNodeServer.ByIds(new[] { selGraphNodeId });
        var selGraphNode = selGraphNodes.Single();
        var expGraphEdgesPage = await _graphEdgeServer.ByNodeIds(new[] { selGraphNode.Id }, 1, int.MaxValue, null);
        var expGraphEdges = expGraphEdgesPage.Items;
        var expGraphEdgeIds = expGraphEdges.Select(ge => ge.Id);
        var expGraphNodeIds = expGraphEdges.SelectMany(ge => new[] { ge.GraphSourceId, ge.GraphTargetId }).Distinct();

        // work out what GraphNodes we already have in Diagram
        var chartNodes = _diagram.Nodes.OfType<DiagramNode>().Select(diagNode => diagNode.ChartNode);
        var graphNodeIds = chartNodes.Select(cn => cn.GraphItemId);

        // work out missing GraphNodes = already in Diagram but not in expansion
        var missGraphNodeIds = expGraphNodeIds.Except(graphNodeIds).ToList();
        var missChartNodes = (await _chartNodeServer.ByGraphItems(_chartId, missGraphNodeIds)).ToList();
        if (missChartNodes.Count() != missGraphNodeIds.Count())
        {
          // GraphNode.Id of ChartNodes which are in Repository
          var repoChartGraphNodeIds = missChartNodes.Select(cn => cn.GraphItemId);

          // get GraphNode.Id of ChartNodes which are not in Repository
          var missRepoChartGraphNodeIds = missGraphNodeIds.Except(repoChartGraphNodeIds);

          // create a ChartNode in Repository for each missing GraphNode
          var missRepoChartGraphNodes = await _graphNodeServer.ByIds(missRepoChartGraphNodeIds);
          var missRepoChartNodes = missRepoChartGraphNodes.Select(gn =>
            new ChartNode(_chartId, _orgId, gn.Id, gn.Name)).ToList();
          _ = await _chartNodeServer.Create(missRepoChartNodes);
          missChartNodes.AddRange(missRepoChartNodes);
        }


        // work out what GraphEdges we already have in Diagram
        var chartEdges = _diagram.Links.OfType<DiagramLink>().Select(diagLink => diagLink.ChartEdge);
        var graphEdgeIds = chartEdges.Select(ce => ce.GraphItemId);

        // work out missing GraphEdges = already in Diagram but not in expansion
        var missGraphEdgeIds = expGraphEdgeIds.Except(graphEdgeIds).ToList();
        var missChartEdges = (await _chartEdgeServer.ByGraphItems(_chartId, missGraphEdgeIds)).ToList();
        if (missChartEdges.Count() != missGraphEdgeIds.Count())
        {
          // GraphEdge.Id of ChartEdges which are in Repository
          var repoChartGraphEdgeIds = missChartEdges.Select(ce => ce.GraphItemId);

          // get GraphEdge.Id of ChartEdges which are not in Repository
          var missRepoChartGraphEdgeIds = missGraphEdgeIds.Except(repoChartGraphEdgeIds);

          // create a ChartEdge in Repository for each missing GraphEdge
          var missRepoChartGraphEdges = await _graphEdgeServer.ByIds(missRepoChartGraphEdgeIds);
          var missRepoChartEdges = new ConcurrentBag<ChartEdge>();
          await missRepoChartGraphEdges.ParallelForEachAsync(DegreeOfParallelism, async ge =>
          {
            var chartNodeSources = await _chartNodeServer.ByGraphItems(_chartId, new[] { ge.GraphSourceId });
            var chartNodeSource = chartNodeSources.Single();
            var chartNodeTargets = await _chartNodeServer.ByGraphItems(_chartId, new[] { ge.GraphTargetId });
            var chartNodeTarget = chartNodeTargets.Single();
            var chartEdge = new ChartEdge(_chartId, _orgId, ge.Id, ge.Name, chartNodeSource.Id, chartNodeTarget.Id);

            missRepoChartEdges.Add(chartEdge);
          });
          _ = await _chartEdgeServer.Create(missRepoChartEdges);
          missChartEdges.AddRange(missRepoChartEdges);
        }

        // create missing nodes
        var nodes = missChartNodes.Select(chartNode =>
          new DiagramNode(chartNode, new Point(chartNode.X, chartNode.Y))
          {
            IconName = chartNode.IconName
          });
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
      finally
      {
        _isBusy = false;
      }
    }

    private async Task OnShowParentChild(ItemClickEventArgs e)
    {
      try
      {
        _isBusy = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        var selChartNode = _diagram.GetSelectedModels().OfType<DiagramNode>().Single();
        var selGraphNodeId = selChartNode.ChartNode.GraphItemId;
        var selGraphNodes = await _graphNodeServer.ByIds(new[] { selGraphNodeId });
        var selGraphNode = selGraphNodes.Single();
        var parentsPage = await _nodeServer.GetParents(selGraphNode.RepositoryItemId, 1, int.MaxValue, null);
        _parentNodes = parentsPage.Items;
        var thisNodePage = await _nodeServer.ByIds(new[] { selGraphNode.RepositoryItemId });
        _selectedNode = thisNodePage.Single();
        var children = await _nodeServer.ByIds(new[] { _selectedNode?.NextId ?? Guid.Empty });
        _childNode = children.SingleOrDefault();
      }
      finally
      {
        _isBusy = false;
      }

      _parentChildDialogIsOpen = true;
    }

    #region Save

    private async Task OnSave()
    {
      try
      {
        _isBusy = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        // get all ChartNodes in this Diagram
        var chartNodes = _diagram.Nodes.OfType<DiagramNode>().Select(dn =>
        {
          dn.ChartNode.X = (int) dn.Position.X;
          dn.ChartNode.Y = (int) dn.Position.Y;
          dn.ChartNode.IconName = dn.IconName?.ToString();
          return dn.ChartNode;
        }).ToList();

        // get all ChartNodes from Repository
        var allChartNodes = await GetChartNodes();
        var missNodeIds = await DeleteMissingNodes(allChartNodes, chartNodes);

        await DeleteMissingEdges();

        await DeleteDanglingDiagramLinks(missNodeIds);

        // only save ChartNodes in Diagram
        // TODO   chunk
        await _chartNodeServer.Update(chartNodes);

        await SaveRenamedNodes(allChartNodes, chartNodes);


        // get all ChartEdges in this Diagram
        var chartEdges = _diagram.Links.OfType<DiagramLink>().Select(dl => dl.ChartEdge).ToList();

        // get all ChartEdges from Repository
        var allChartEdges = await GetChartEdges();

        await SaveRenamedEdges(allChartEdges, chartEdges);
      }
      finally
      {
        _isBusy = false;
      }
    }

    private async Task<List<Guid>> DeleteMissingNodes(List<ChartNode> allChartNodes, List<ChartNode> chartNodes)
    {
      var allChartNodeIds = allChartNodes.Select(cn => cn.Id);
      var chartNodeIds = chartNodes.Select(cn => cn.Id);

      // work out which ChartNodes are in Repository but not in Diagram ie deleted from Diagram
      var missNodeIds = allChartNodeIds.Except(chartNodeIds).ToList();
      var missNodes = allChartNodes.Where(cn => missNodeIds.Contains(cn.Id));

      // delete missing ChartNodes from Repository
      // TODO   chunk
      await _chartNodeServer.Delete(missNodes);

      return missNodeIds;
    }

    private async Task DeleteMissingEdges()
    {
      // get all ChartEdges from Repository
      var allChartEdges = await GetChartEdges();
      var allChartEdgeNodeIds = allChartEdges.Select(ce => ce.Id);

      // get all ChartEdges in this Diagram
      var chartEdges = _diagram.Links.OfType<DiagramLink>().Select(dl => dl.ChartEdge);
      var chartEdgeIds = chartEdges.Select(ce => ce.Id);

      // work out which ChartEdges are in Repository but not in Diagram ie delete from Diagram
      var missEdgeIds = allChartEdgeNodeIds.Except(chartEdgeIds);
      var missEdges = allChartEdges.Where(ce => missEdgeIds.Contains(ce.Id));

      // delete missing ChartEdges from Repository
      // TODO   chunk
      await _chartEdgeServer.Delete(missEdges);
    }

    private async Task DeleteDanglingDiagramLinks(List<Guid> missNodeIds)
    {
      // delete dangling DiagramLinks
      // BUG:   portless charts do not seem to delete links when an attached node is deleted
      //        Further, the attached node does not appear to be removed from the link!
      var dangChartEdges = _diagram.Links.OfType<DiagramLink>()
        .Where(dl => missNodeIds.Contains(dl.ChartEdge.ChartSourceId) || missNodeIds.Contains(dl.ChartEdge.ChartTargetId))
        .Select(dl => dl.ChartEdge);
      // TODO   chunk
      await _chartEdgeServer.Delete(dangChartEdges);
    }

    private async Task SaveRenamedNodes(List<ChartNode> allChartNodes, List<ChartNode> chartNodes)
    {
      var changedNameChartNodes = chartNodes
        .Where(cn => allChartNodes.Single(acn => cn.Id == acn.Id).Name != cn.Name).ToList();
      var changedNameGraphNodeIds = changedNameChartNodes
        .Select(cn => cn.GraphItemId);
      // TODO   chunk
      var changedNameGraphNodes = (await _graphNodeServer.ByIds(changedNameGraphNodeIds)).ToList();
      var changedNameRepoGraphNodeIds = changedNameGraphNodes.Select(gn => gn.RepositoryItemId);
      // TODO   chunk
      var changedNameRepoNodes = (await _nodeServer.ByIds(changedNameRepoGraphNodeIds)).ToList();
      changedNameGraphNodes
        .ForEach(gn => gn.Name = chartNodes.Single(cn => cn.GraphItemId == gn.Id).Name);
      changedNameRepoNodes
        .ForEach(rn => rn.Name = changedNameGraphNodes.Single(gn => gn.RepositoryItemId == rn.Id).Name);
      // TODO   chunk
      await _chartNodeServer.Update(changedNameChartNodes);
      // TODO   chunk
      await _graphNodeServer.Update(changedNameGraphNodes);
      // TODO   chunk
      await _nodeServer.Update(changedNameRepoNodes);
    }

    private async Task SaveRenamedEdges(List<ChartEdge> allChartEdges, List<ChartEdge> chartEdges)
    {
      var changedNameChartEdges = chartEdges
        .Where(cn => allChartEdges.Single(acn => cn.Id == acn.Id).Name != cn.Name).ToList();
      var changedNameGraphEdgeIds = changedNameChartEdges
        .Select(cn => cn.GraphItemId);
      // TODO   chunk
      var changedNameGraphEdges = (await _graphEdgeServer.ByIds(changedNameGraphEdgeIds)).ToList();
      var changedNameRepoGraphEdgeIds = changedNameGraphEdges.Select(gn => gn.RepositoryItemId);
      // TODO   chunk
      var changedNameRepoEdges = (await _edgeServer.ByIds(changedNameRepoGraphEdgeIds)).ToList();
      changedNameGraphEdges
        .ForEach(gn => gn.Name = chartEdges.Single(cn => cn.GraphItemId == gn.Id).Name);
      changedNameRepoEdges
        .ForEach(rn => rn.Name = changedNameGraphEdges.Single(gn => gn.RepositoryItemId == rn.Id).Name);
      // TODO   chunk
      await _chartEdgeServer.Update(changedNameChartEdges);
      // TODO   chunk
      await _graphEdgeServer.Update(changedNameGraphEdges);
      // TODO   chunk
      await _edgeServer.Update(changedNameRepoEdges);
    }

    #endregion

    private async Task OnLayout(string layout)
    {
      if (string.IsNullOrWhiteSpace(layout))
      {
        return;
      }

      try
      {
        _isBusy = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        var graph = new QG.BidirectionalGraph<DiagramNode, QG.Edge<DiagramNode>>();
        var nodes = _diagram.Nodes.OfType<DiagramNode>().ToList();
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
            vertPos.Key.Refresh();
          }
        }
        finally
        {
          _diagram.SuspendRefresh = false;
        }
      }
      finally
      {
        _isBusy = false;
      }
    }

    private void OnShowLabel(bool value)
    {
      _diagram
        .Links
        .OfType<DiagramLink>()
        .SelectMany(dl => dl.Labels)
        .OfType<DiagramLinkLabel>()
        .ToList()
        .ForEach(dll => { dll.ShowLabel = value; });
    }

    private Action<NodeModel> UpdateDiagramNodes()
    {
      return (_ =>
      {
        _diagNodes = _diagram.Nodes.OfType<DiagramNode>().ToList();
        StateHasChanged();
      });
    }

    #region Zoom

    private void CentreOnNode(DiagramNode node)
    {
      // TODO   not perfect but good enough for the moment
      var cont = _diagram.Container ?? Rectangle.Zero;
      var deltaX = -_diagram.Pan.X + (-node.Position.X + cont.Width / 2 - cont.Left);
      var deltaY = -_diagram.Pan.Y + (-node.Position.Y + cont.Height / 2 - cont.Top);
      _diagram.UpdatePan(deltaX, deltaY);
    }

    private void ZoomIn()
    {
      _diagram.SetZoom(_diagram.Zoom * 1.15);
    }

    private void ZoomOut()
    {
      _diagram.SetZoom(_diagram.Zoom * 0.85);
    }

    private void ZoomReset()
    {
      _diagram.SetZoom(1.0);
    }

    private void ZoomToFit()
    {
      _diagram.ZoomToFit();
    }

    #endregion
  }
}
