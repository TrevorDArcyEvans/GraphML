using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuickGraph;
using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class BetweennessJob : JobBase, IBetweennessJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<BetweennessJob> _logger;
    private readonly IEdgeDatastore _edgeDatastore;
    private readonly INodeDatastore _nodeDatastore;
    private readonly ICentralityBetweennessAlgorithmFactory _factory;
    private readonly IResultLogic _resultLogic;

    public BetweennessJob(
      IConfiguration config,
      ILogger<BetweennessJob> logger,
      IEdgeDatastore edgeDatastore,
      INodeDatastore nodeDatastore,
      ICentralityBetweennessAlgorithmFactory factory,
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
      var closeReq = (IBetweennessRequest)req;

      var graph = new BidirectionalGraph<string, IEdge<string>>();

      // TODO   retrieve nodes from db
      #region Create the nodes
      var A = "Andre";
      var B = "Beverley";
      var C = "Carol";
      var D = "Diane";
      var E = "Ed";
      var F = "Fernando";
      var G = "Garth";
      var H = "Heather";
      var I = "Ike";
      var J = "Jane";
      #endregion

      #region Add some vertices to the graph
      graph.AddVertex(A);
      graph.AddVertex(B);
      graph.AddVertex(C);
      graph.AddVertex(D);
      graph.AddVertex(E);
      graph.AddVertex(F);
      graph.AddVertex(G);
      graph.AddVertex(H);
      graph.AddVertex(I);
      graph.AddVertex(J);
      #endregion

      // TODO   retrieve edges from db
      #region Create the edges
      var a_b = new Edge<string>(A, B);
      var a_c = new Edge<string>(A, C);
      var a_d = new Edge<string>(A, D);
      var a_f = new Edge<string>(A, F);

      var b_d = new Edge<string>(B, D);
      var b_e = new Edge<string>(B, E);
      var b_g = new Edge<string>(B, G);

      var c_d = new Edge<string>(C, D);
      var c_f = new Edge<string>(C, F);

      var d_e = new Edge<string>(D, E);
      var d_f = new Edge<string>(D, F);
      var d_g = new Edge<string>(D, G);

      var e_g = new Edge<string>(E, G);

      var f_g = new Edge<string>(F, G);
      var f_h = new Edge<string>(F, H);

      var g_h = new Edge<string>(G, H);

      var h_i = new Edge<string>(H, I);

      var i_j = new Edge<string>(I, J);
      #endregion

      #region Add the edges
      graph.AddEdge(a_b); graph.AddEdge(new Edge<string>(B, A));
      graph.AddEdge(a_c); graph.AddEdge(new Edge<string>(C, A));
      graph.AddEdge(a_d); graph.AddEdge(new Edge<string>(D, A));
      graph.AddEdge(a_f); graph.AddEdge(new Edge<string>(F, A));

      graph.AddEdge(b_d); graph.AddEdge(new Edge<string>(D, B));
      graph.AddEdge(b_e); graph.AddEdge(new Edge<string>(E, B));
      graph.AddEdge(b_g); graph.AddEdge(new Edge<string>(G, B));

      graph.AddEdge(c_f); graph.AddEdge(new Edge<string>(F, C));
      graph.AddEdge(c_d); graph.AddEdge(new Edge<string>(D, C));

      graph.AddEdge(d_e); graph.AddEdge(new Edge<string>(E, D));
      graph.AddEdge(d_f); graph.AddEdge(new Edge<string>(F, D));
      graph.AddEdge(d_g); graph.AddEdge(new Edge<string>(G, D));

      graph.AddEdge(e_g); graph.AddEdge(new Edge<string>(G, E));

      graph.AddEdge(f_g); graph.AddEdge(new Edge<string>(G, F));
      graph.AddEdge(f_h); graph.AddEdge(new Edge<string>(H, F));

      graph.AddEdge(g_h); graph.AddEdge(new Edge<string>(H, G));

      graph.AddEdge(h_i); graph.AddEdge(new Edge<string>(I, H));

      graph.AddEdge(i_j); graph.AddEdge(new Edge<string>(J, I));
      #endregion

      // use unweighted edges
      var algo = _factory.Create(graph, e => 1.0);

      var result = algo.Compute();

      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
