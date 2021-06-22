using FluentAssertions;

namespace Comuna.Tests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class HighlyConnectedSubgraphs_Tests
  {
/*
Initial graph
    https://en.wikipedia.org/wiki/HCS_clustering_algorithm

                          (4)
                          / \
                         /   \
                        /     \
    (1)-----(2)-------(3)-----(5)
    /|      /|         |       |
   / |     / |         |       |
  /  |    /  |         |       |
(0)--|--\/   |         |       |
  \  |  /\   |         |       |
   \ | /  \  |         |       |
    \|/    \ |         |       |
   (11)----(10)-------(9)-----(6)
                       |\     /|
                       | \   / |
                       |  \ /  |
                       |   X   |
                       |  / \  |
                       | /   \ |
                       |/     \|
                      (8)-----(7)
*/

/*
Results

                          (4)
                          / \
                         /   \
                        /     \
    (1)-----(2)       (3)-----(5)
    /|      /|
   / |     / |
  /  |    /  |
(0)--|--\/   |
  \  |  /\   |
   \ | /  \  |
    \|/    \ |
   (11)----(10)       (9)-----(6)
                       |\     /|
                       | \   / |
                       |  \ /  |
                       |   X   |
                       |  / \  |
                       | /   \ |
                       |/     \|
                      (8)-----(7)

Community: 0
  Node: 0
  Node: 1
  Node: 2
  Node: 10
  Node: 11
Community: 1
  Node: 6
  Node: 7
  Node: 8
  Node: 9
Community: 2
  Node: 3
  Node: 4
  Node: 5
*/

    [TestMethod]
    public void Algorithm_Returns_Expected_Number_Communities()
    {
      var network = CreateNetwork();
      var communityAlg = new CommunityAlgorithm(network);

      communityAlg.Update();

      var communityNodes = communityAlg.GetCommunityNodes();
      communityNodes.Count().Should().Be(3);
    }

    [TestMethod]
    public void Algorithm_Returns_Communities_With_Expected_Number_Nodes()
    {
      var network = CreateNetwork();
      var communityAlg = new CommunityAlgorithm(network);

      communityAlg.Update();

      var communityNodes = communityAlg.GetCommunityNodes().ToList();
      communityNodes.Single(cn => cn.Key == 0).Count().Should().Be(5);
      communityNodes.Single(cn => cn.Key == 1).Count().Should().Be(4);
      communityNodes.Single(cn => cn.Key == 2).Count().Should().Be(3);
    }

    [DataTestMethod]
    [DataRow(0, new[] { 0, 1, 2, 10, 11 })]
    [DataRow(1, new[] { 6, 7, 8, 9 })]
    [DataRow(2, new[] { 3, 4, 5 })]
    public void Algorithm_Returns_Community_With_Expected_Nodes(int community, int[] nodeIds)
    {
      var network = CreateNetwork();
      var communityAlg = new CommunityAlgorithm(network);

      communityAlg.Update();

      var communityNodes = communityAlg.GetCommunityNodes().ToList();
      communityNodes
        .Single(cn => cn.Key == community)
        .Select(cn => cn.Node)
        .Should()
        .BeEquivalentTo(nodeIds);
    }

    private static Network CreateNetwork()
    {
      var network = new Network();
      foreach (var node in Enumerable.Range(0, 12))
      {
        network.AddVertex((uint) node);
      }

      #region edges

      network.AddEdge(new Connection(0, 1));
      network.AddEdge(new Connection(0, 11));
      network.AddEdge(new Connection(0, 10));
      network.AddEdge(new Connection(1, 2));
      network.AddEdge(new Connection(1, 11));
      network.AddEdge(new Connection(2, 10));
      network.AddEdge(new Connection(2, 11));
      network.AddEdge(new Connection(10, 11));

      network.AddEdge(new Connection(3, 4));
      network.AddEdge(new Connection(4, 5));
      network.AddEdge(new Connection(3, 5));

      network.AddEdge(new Connection(6, 9));
      network.AddEdge(new Connection(8, 9));
      network.AddEdge(new Connection(7, 9));
      network.AddEdge(new Connection(6, 8));
      network.AddEdge(new Connection(6, 7));
      network.AddEdge(new Connection(7, 8));

      network.AddEdge(new Connection(2, 3));
      network.AddEdge(new Connection(9, 10));

      network.AddEdge(new Connection(3, 9));
      network.AddEdge(new Connection(5, 6));

      #endregion

      return network;
    }
  }
}
