using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuickGraph;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeJob : JobBase, IDegreeJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<DegreeJob> _logger;
    private readonly IGraphDatastore _graphDatastore;
    private readonly IResultLogic _resultLogic;

    public DegreeJob(
      IConfiguration config,
      ILogger<DegreeJob> logger,
      IGraphDatastore graphDatastore,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _graphDatastore = graphDatastore;
      _resultLogic = resultLogic;
    }

    public override void Run(IRequest req)
    {
      var degReq = (DegreeRequest)req;
      _logger.LogInformation($"DegreeJob.Run --> {degReq.GraphId} @ {degReq.CorrelationId}");

      #region Create the edges
      var a_b = new Edge<string>("A", "B");
      var a_d = new Edge<string>("A", "D");
      var b_a = new Edge<string>("B", "A");
      var b_c = new Edge<string>("B", "C");
      var b_e = new Edge<string>("B", "E");
      var c_b = new Edge<string>("C", "B");
      var c_f = new Edge<string>("C", "F");
      var c_j = new Edge<string>("C", "J");
      var d_e = new Edge<string>("D", "E");
      var d_g = new Edge<string>("D", "G");
      var e_d = new Edge<string>("E", "D");
      var e_f = new Edge<string>("E", "F");
      var e_h = new Edge<string>("E", "H");
      var f_i = new Edge<string>("F", "I");
      var f_j = new Edge<string>("F", "J");
      var g_d = new Edge<string>("G", "D");
      var g_h = new Edge<string>("G", "H");
      var h_g = new Edge<string>("H", "G");
      var h_i = new Edge<string>("H", "I");
      var i_f = new Edge<string>("I", "F");
      var i_j = new Edge<string>("I", "J");
      var i_h = new Edge<string>("I", "H");
      var j_f = new Edge<string>("J", "F");

      var f_k = new Edge<string>("F", "K");
      var k_i = new Edge<string>("K", "I");
      #endregion

      #region Define some weights to the edges
      var edgeCost = new Dictionary<IEdge<string>, double>
      {
        { a_b,  4 },
        { a_d,  1 },
        { b_a, 74 },
        { b_c,  2 },
        { b_e, 12 },
        { c_b, 12 },
        { c_f, 74 },
        { c_j, 12 },
        { d_e, 32 },
        { d_g, 22 },
        { e_d, 66 },
        { e_f, 76 },
        { e_h, 33 },
        { f_i, 11 },
        { f_j, 21 },
        { g_d, 12 },
        { g_h, 10 },
        { h_g,  2 },
        { h_i, 72 },
        { i_f, 31 },
        { i_h, 18 },
        { i_j,  7 },
        { j_f,  8 },

        { f_k,  3 },
        { k_i,  8 }
      };
      #endregion

      #region Add edges to graph
      var graph = new EdgeListGraph<string, IEdge<string>>();
      graph.AddEdgeRange(new[]
      {
        a_b,
        a_d,
        b_a,
        b_c,
        b_e,
        c_b,
        c_f,
        c_j,
        d_e,
        d_g,
        e_d,
        e_f,
        e_h,
        f_i,
        f_j,
        g_d,
        g_h,
        h_g,
        h_i,
        i_f,
        i_j,
        i_h,
        j_f,

        f_k,
        k_i
      });
      #endregion

      var algo = new Degree<string, IEdge<string>>(graph, e => edgeCost[e]);
      var resultsList = new List<DegreeVertexResult<string>>();
      algo.VertexResult += vertexRes => resultsList.Add(vertexRes);

      algo.Compute();

      var result = new DegreeResult<string>(resultsList);
      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
