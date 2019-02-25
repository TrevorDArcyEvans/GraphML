using FluentAssertions;
using NUnit.Framework;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Analysis.SNA.Centrality.Tests
{
  [TestFixture]
  public sealed class Betweenness_Tests
  {
    private BidirectionalGraph<string, IEdge<string>> _graph;
    Func<IEdge<string>, double> _weights;

    [SetUp]
    public void SetUp()
    {
      _graph = new BidirectionalGraph<string, IEdge<string>>();
      _weights = e => 1.0;
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new Betweenness<string, IEdge<string>>(_graph, _weights));
    }

    [Test]
    public void Compute_EmptyGraph_DoesNotHang()
    {
      _graph.RemoveEdgeIf(e => true);
      var algo = new Betweenness<string, IEdge<string>>(_graph, _weights);

      algo.Compute();

      Assert.Pass();
    }

    /// <summary>
    /// Kite social network from:
    ///     http://www.orgnet.com/sna.html
    /// </summary>
    [Test]
    public void Compute_Kite_ReturnsExpected()
    {
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
      _graph.AddVertex(A);
      _graph.AddVertex(B);
      _graph.AddVertex(C);
      _graph.AddVertex(D);
      _graph.AddVertex(E);
      _graph.AddVertex(F);
      _graph.AddVertex(G);
      _graph.AddVertex(H);
      _graph.AddVertex(I);
      _graph.AddVertex(J);
      #endregion

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
      _graph.AddEdge(a_b); _graph.AddEdge(new Edge<string>(B, A));
      _graph.AddEdge(a_c); _graph.AddEdge(new Edge<string>(C, A));
      _graph.AddEdge(a_d); _graph.AddEdge(new Edge<string>(D, A));
      _graph.AddEdge(a_f); _graph.AddEdge(new Edge<string>(F, A));

      _graph.AddEdge(b_d); _graph.AddEdge(new Edge<string>(D, B));
      _graph.AddEdge(b_e); _graph.AddEdge(new Edge<string>(E, B));
      _graph.AddEdge(b_g); _graph.AddEdge(new Edge<string>(G, B));

      _graph.AddEdge(c_f); _graph.AddEdge(new Edge<string>(F, C));
      _graph.AddEdge(c_d); _graph.AddEdge(new Edge<string>(D, C));

      _graph.AddEdge(d_e); _graph.AddEdge(new Edge<string>(E, D));
      _graph.AddEdge(d_f); _graph.AddEdge(new Edge<string>(F, D));
      _graph.AddEdge(d_g); _graph.AddEdge(new Edge<string>(G, D));

      _graph.AddEdge(e_g); _graph.AddEdge(new Edge<string>(G, E));

      _graph.AddEdge(f_g); _graph.AddEdge(new Edge<string>(G, F));
      _graph.AddEdge(f_h); _graph.AddEdge(new Edge<string>(H, F));

      _graph.AddEdge(g_h); _graph.AddEdge(new Edge<string>(H, G));

      _graph.AddEdge(h_i); _graph.AddEdge(new Edge<string>(I, H));

      _graph.AddEdge(i_j); _graph.AddEdge(new Edge<string>(J, I));
      #endregion

      var algo = new Betweenness<string, IEdge<string>>(_graph, _weights);

      var results = algo.Compute();

      var expected = new Dictionary<string, double>
      {
        { H, 28 },
        { G, 22 },
        { F, 22 },
        { I, 16 },
        { D, 14 },
        { A,  4 },
        { B,  4 }
      };
      results.Count().Should().Be(expected.Keys.Count);
      foreach (var result in results)
      {
        expected[result.Vertex].Should().BeApproximately(result.Betweenness, 1e-12);
      }
    }
  }
}
