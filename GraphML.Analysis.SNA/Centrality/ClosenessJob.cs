using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuickGraph;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class ClosenessJob : JobBase, IClosenessJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<ClosenessJob> _logger;
    private readonly IEdgeDatastore _edgeDatastore;
    private readonly INodeDatastore _nodeDatastore;
    private readonly ICentralityClosenessAlgorithmFactory _factory;
    private readonly IResultLogic _resultLogic;

    public ClosenessJob(
      IConfiguration config,
      ILogger<ClosenessJob> logger,
      IEdgeDatastore edgeDatastore,
      INodeDatastore nodeDatastore,
      ICentralityClosenessAlgorithmFactory factory,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _edgeDatastore = edgeDatastore;
      _nodeDatastore = nodeDatastore;
      _factory = factory;
      _resultLogic = resultLogic;
    }

    public override void Run(IRequest req)
    {
      var closeReq = (IClosenessRequest)req;

      // TODO   retreive edges from db
      // TODO   retreive nodes from db
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
      var edgeCost = new Dictionary<IEdge<string>, double>();
      edgeCost.Add(a_b, 4);
      edgeCost.Add(a_d, 1);
      edgeCost.Add(b_a, 74);
      edgeCost.Add(b_c, 2);
      edgeCost.Add(b_e, 12);
      edgeCost.Add(c_b, 12);
      edgeCost.Add(c_f, 74);
      edgeCost.Add(c_j, 12);
      edgeCost.Add(d_e, 32);
      edgeCost.Add(d_g, 22);
      edgeCost.Add(e_d, 66);
      edgeCost.Add(e_f, 76);
      edgeCost.Add(e_h, 33);
      edgeCost.Add(f_i, 11);
      edgeCost.Add(f_j, 21);
      edgeCost.Add(g_d, 12);
      edgeCost.Add(g_h, 10);
      edgeCost.Add(h_g, 2);
      edgeCost.Add(h_i, 72);
      edgeCost.Add(i_f, 31);
      edgeCost.Add(i_h, 18);
      edgeCost.Add(i_j, 7);
      edgeCost.Add(j_f, 8);

      edgeCost.Add(f_k, 3);
      edgeCost.Add(k_i, 8);
      #endregion

      var _graph = new AdjacencyGraph<string, IEdge<string>>();
      _graph.AddVertexRange(
        new[]
        {
          "A",
          "B",
          "C",
          "D",
          "E",
          "F",
          "G",
          "H",
          "I",
          "J",
          "K"
        });
      _graph.AddEdgeRange(
        new[]
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
          k_i,
        });

      var algo = _factory.Create(_graph, e => edgeCost[e]);
      var resultsList = new List<ClosenessVertexResult<string>>();
      algo.VertexResult += vertexRes => resultsList.Add(vertexRes);

      algo.Compute();

      var result = new ClosenessResult<string>(resultsList);
      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
