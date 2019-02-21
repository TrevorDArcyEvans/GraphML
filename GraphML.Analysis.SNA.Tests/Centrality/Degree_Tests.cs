using FluentAssertions;
using Moq;
using NUnit.Framework;
using QuickGraph;
using System;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality.Tests
{
  [TestFixture]
  public sealed class Degree_Tests
  {
    #region Create the edges
    Edge<string> a_b = new Edge<string>("A", "B");
    Edge<string> a_d = new Edge<string>("A", "D");
    Edge<string> b_a = new Edge<string>("B", "A");
    Edge<string> b_c = new Edge<string>("B", "C");
    Edge<string> b_e = new Edge<string>("B", "E");
    Edge<string> c_b = new Edge<string>("C", "B");
    Edge<string> c_f = new Edge<string>("C", "F");
    Edge<string> c_j = new Edge<string>("C", "J");
    Edge<string> d_e = new Edge<string>("D", "E");
    Edge<string> d_g = new Edge<string>("D", "G");
    Edge<string> e_d = new Edge<string>("E", "D");
    Edge<string> e_f = new Edge<string>("E", "F");
    Edge<string> e_h = new Edge<string>("E", "H");
    Edge<string> f_i = new Edge<string>("F", "I");
    Edge<string> f_j = new Edge<string>("F", "J");
    Edge<string> g_d = new Edge<string>("G", "D");
    Edge<string> g_h = new Edge<string>("G", "H");
    Edge<string> h_g = new Edge<string>("H", "G");
    Edge<string> h_i = new Edge<string>("H", "I");
    Edge<string> i_f = new Edge<string>("I", "F");
    Edge<string> i_j = new Edge<string>("I", "J");
    Edge<string> i_h = new Edge<string>("I", "H");
    Edge<string> j_f = new Edge<string>("J", "F");

    Edge<string> f_k = new Edge<string>("F", "K");
    Edge<string> k_i = new Edge<string>("K", "I");
    #endregion

    Dictionary<IEdge<string>, double> _edgeCost;

    private Mock<IEdgeSet<string, IEdge<string>>> _edgeSet;
    Func<IEdge<string>, double> _weights;

    [SetUp]
    public void SetUp()
    {
      #region Define some weights to the edges
      _edgeCost = new Dictionary<IEdge<string>, double>();
      _edgeCost.Add(a_b, 4);
      _edgeCost.Add(a_d, 1);
      _edgeCost.Add(b_a, 74);
      _edgeCost.Add(b_c, 2);
      _edgeCost.Add(b_e, 12);
      _edgeCost.Add(c_b, 12);
      _edgeCost.Add(c_f, 74);
      _edgeCost.Add(c_j, 12);
      _edgeCost.Add(d_e, 32);
      _edgeCost.Add(d_g, 22);
      _edgeCost.Add(e_d, 66);
      _edgeCost.Add(e_f, 76);
      _edgeCost.Add(e_h, 33);
      _edgeCost.Add(f_i, 11);
      _edgeCost.Add(f_j, 21);
      _edgeCost.Add(g_d, 12);
      _edgeCost.Add(g_h, 10);
      _edgeCost.Add(h_g, 2);
      _edgeCost.Add(h_i, 72);
      _edgeCost.Add(i_f, 31);
      _edgeCost.Add(i_h, 18);
      _edgeCost.Add(i_j, 7);
      _edgeCost.Add(j_f, 8);

      _edgeCost.Add(f_k, 3);
      _edgeCost.Add(k_i, 8);
      #endregion

      _edgeSet = new Mock<IEdgeSet<string, IEdge<string>>>();
      _edgeSet.Setup(x => x.Edges).Returns(
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
          k_i
        });

      _weights = e => _edgeCost[e];
    }

    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new Degree<string, IEdge<string>>(_edgeSet.Object, _weights));
    }

    [Test]
    public void Compute_RaisesEvent_ForEachNode()
    {
      var nodeCount = 0;
      var algo = new Degree<string, IEdge<string>>(_edgeSet.Object, _weights);
      algo.VertexResult += result => nodeCount++;

      algo.Compute();

      nodeCount.Should().Be(11);
    }

    [Test]
    public void Compute_ReturnsExpected()
    {
      var algo = new Degree<string, IEdge<string>>(_edgeSet.Object, _weights);
      var results = new List<DegreeResult<string>>();
      algo.VertexResult += result => results.Add(result);

      algo.Compute();

      results.Should()
        .HaveCount(11)
        .And
        .BeEquivalentTo(
          new[]
          {
            new DegreeResult<string>("A",  74,   5),
            new DegreeResult<string>("B",  16,  88),
            new DegreeResult<string>("D",  79,  54),
            new DegreeResult<string>("C",   2,  98),
            new DegreeResult<string>("E",  44, 175),
            new DegreeResult<string>("F", 189,  35),
            new DegreeResult<string>("G",  24,  22),
            new DegreeResult<string>("H",  61,  74),
            new DegreeResult<string>("I",  91,  56),
            new DegreeResult<string>("J",  40,   8),
            new DegreeResult<string>("K",   3,   8)
          });
    }
  }
}
