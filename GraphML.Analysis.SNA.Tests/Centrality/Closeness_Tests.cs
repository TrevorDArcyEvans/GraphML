using FluentAssertions;
using NUnit.Framework;
using QuikGraph;
using System;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality.Tests
{
  [TestFixture]
  public sealed class Closeness_Tests
  {
    private AdjacencyGraph<string, IEdge<string>> _graph;
    private Func<IEdge<string>, double> _weights;

    [SetUp]
    public void SetUp()
    {
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

      _graph = new AdjacencyGraph<string, IEdge<string>>();
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

      _weights = e => edgeCost[e];
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new Closeness<string, IEdge<string>>(_graph, _weights));
    }

    [Test]
    public void Compute_EmptyGraph_DoesNotHang()
    {
      _graph.RemoveEdgeIf(e => true);
      var algo = new Closeness<string, IEdge<string>>(_graph, _weights);

      algo.Compute();

      Assert.Pass();
    }

    [Test]
    public void Compute_RaisesEvent_ForEachNode()
    {
      var nodeCount = 0;
      var algo = new Closeness<string, IEdge<string>>(_graph, _weights);
      algo.VertexResult += result => nodeCount++;

      algo.Compute();

      nodeCount.Should().Be(11);
    }

    [Test]
    public void Compute_ReturnsExpected()
    {
      var algo = new Closeness<string, IEdge<string>>(_graph, _weights);
      var results = new Dictionary<string, double>();
      algo.VertexResult += result => results.Add(result.Vertex, result.Closeness);

      algo.Compute();

      var expected = new Dictionary<string, double>
      {
        { "A", 0.0518134715025907 },
        { "B", 0.0300300300300300 },
        { "C", 0.0269541778975741 },
        { "D", 0.0192307692307692 },
        { "E", 0.0221729490022173 },
        { "F", 0.0476190476190476 },
        { "G", 0.0230414746543779 },
        { "H", 0.0256410256410256 },
        { "I", 0.0574712643678161 },
        { "J", 0.0403225806451613 },
        { "K", 0.0471698113207547 }
      };
      results.Keys.Count.Should().Be(expected.Keys.Count);
      foreach (var result in results)
      {
        expected[result.Key].Should().BeApproximately(result.Value, 1e-12);
      }
    }
  }
}
