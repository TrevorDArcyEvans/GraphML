using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.GanttChart;
using ChartJs.Blazor.GanttChart.Axes;
using ChartJs.Blazor.Util;
using GraphML.Interfaces.Server;
using GraphML.Utils;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace GraphML.UI.Web.Pages
{
  public partial class ShowTimeLine
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
    public string TimelineName { get; set; }

    [Parameter]
    public string TimelineId { get; set; }

    [Parameter]
    public string EdgeItemAttributeDefinitionId { get; set; }

    [Parameter]
    public string EdgeItemAttributeDefinitionName { get; set; }

    #endregion

    #region Inject

    [Inject]
    private IEdgeServer _edgeServer { get; set; }

    [Inject]
    private IGraphEdgeServer _graphEdgeServer { get; set; }

    [Inject]
    private IEdgeItemAttributeServer _edgeItemAttribServer { get; set; }

    #endregion

    private GanttConfig _config;
    private ChartJs.Blazor.Chart _chart;

    protected override async Task OnInitializedAsync()
    {
      _config = GetConfig();

      var data = await GetData();
      IDataset<GanttDateTimeData> dataset = GetDataset(data);

      _config.Data.Datasets.Add(dataset);
    }

    private GanttConfig GetConfig()
    {
      return new GanttConfig
      {
        Options = new GanttOptions
        {
          Responsive = true,
          Legend = new Legend
          {
            Position = Position.Right
          },
          Title = new OptionsTitle
          {
            Display = true,
            Text = $"{TimelineName}"
          },
          Scales = new GanttScales
          {
            XAxes = new List<GanttTimeAxis>(
              new[]
              {
                new GanttTimeAxis
                {
                  Display = AxisDisplay.True,
                  Position = Position.Bottom,
                  Time = new TimeOptions
                  {
                    DisplayFormats = new Dictionary<TimeMeasurement, string>
                    {
                      { TimeMeasurement.Millisecond, "HH:mm:ss.SSS" },
                      { TimeMeasurement.Second, "HH:mm:ss" },
                      { TimeMeasurement.Minute, "HH:mm:ss" },
                      { TimeMeasurement.Hour, "HH:mm:ss" },
                      { TimeMeasurement.Day, "HH:mm:ss" },
                    }
                  }
                }
              })
          }
        }
      };
    }

    private async Task<List<GanttDateTimeData>> GetData()
    {
      const int ChunkSize = 1000;
      const int DegreeofParallelism = 10;

      var lockObj = new object();

      var graphId = Guid.Parse(GraphId);
      var allGraphEdgesCount = await _graphEdgeServer.Count(graphId);
      var edgeIds = new List<Guid>(allGraphEdgesCount);
      var numGraphEdgeChunks = (allGraphEdgesCount / ChunkSize) + 1;
      var graphEdgeChunkRange = Enumerable.Range(0, numGraphEdgeChunks);
      await graphEdgeChunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
      {
        var graphEdgesPage = await _graphEdgeServer.ByOwner(graphId, i + 1, ChunkSize, null);
        var graphEdges = graphEdgesPage.Items;
        lock (lockObj)
        {
          edgeIds.AddRange(graphEdges.Select(ge => ge.RepositoryItemId));
        }
      });

      var edgeItemAttribDef = Guid.Parse(EdgeItemAttributeDefinitionId);
      var numEdgeItemAttrChunks = edgeIds.Count / ChunkSize + 1;
      var edgeItemAttribs = new List<EdgeItemAttribute>(edgeIds.Count());
      var edgeItemAttrChunkRange = Enumerable.Range(0, numEdgeItemAttrChunks);
      await edgeItemAttrChunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
      {
      var edgeItemAttribsPage = await _edgeItemAttribServer.ByOwners(edgeIds, i + 1, ChunkSize, null);
        lock (lockObj)
        {
          edgeItemAttribs.AddRange(edgeItemAttribsPage.Items.Where(eia => eia.DefinitionId == edgeItemAttribDef));
        }
      });

      var dateTimeIntJson = edgeItemAttribs.Select(eia => eia.DataValueAsString);
      var dateTimeInts = dateTimeIntJson.Select(dtj => JsonConvert.DeserializeObject<DateTimeInterval>(dtj)).ToList();
      var ganttData = new List<GanttDateTimeData>(dateTimeInts.Count);
      var yaxisVal = 0d;
      dateTimeInts.ForEach(dti =>
      {
        var data = new GanttDateTimeData
        {
          X = new GanttDateTimeInterval
          {
            From = dti.Start,
            To = dti.End
          },
          Y = yaxisVal
        };
        ganttData.Add(data);
        yaxisVal += 0.125;
      });

      return ganttData;
    }

    private GanttDateTimeDataset GetDataset(List<GanttDateTimeData> data)
    {
      return new GanttDateTimeDataset(data)
      {
        Label = $"{EdgeItemAttributeDefinitionName}",
        BackgroundColor = ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(128, ChartColors.Red)),
        BorderColor = ColorUtil.FromDrawingColor(ChartColors.Red),
        BorderWidth = 1,
        Width = "1ms"
      };
    }

    public static class ChartColors
    {
      public static readonly System.Drawing.Color Red = System.Drawing.Color.FromArgb(255, 99, 132);
    }
  }
}