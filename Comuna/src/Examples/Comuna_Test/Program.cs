using System;
using System.Collections.Generic;
using System.Linq;
using Comuna;
using Newtonsoft.Json;

namespace Comuna_Test
{
  public class Program
  {
    public static void Main(string[] args)
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

      var network = new Network();
      var nodes = Enumerable.Range(0, 12).ToList();
      foreach (var node in nodes)
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

      var communityAlg = new CommunityAlgorithm(network);

      communityAlg.Update();

      var communityNodes = communityAlg.GetCommunityNodes();
      foreach (var commNodes in communityNodes)
      {
        Console.WriteLine($"Community: {commNodes.Key}");
        foreach (var commNode in commNodes)
        {
          Console.WriteLine($"  Node: {commNode.Node}");
        }
      }

      Console.WriteLine("------------------------------------------");


      // NOTE:  have to deserialise as List<list<>> because GroupedEnumerable is not public
      var json = JsonConvert.SerializeObject(communityNodes);
      var result = JsonConvert.DeserializeObject<List<List<CommunityNode>>>(json);
      foreach (var community in result)
      {
        Console.WriteLine($"Community: {community.First().Community}");
        foreach (var commNode in community)
        {
          Console.WriteLine($"  Node: {commNode.Node}");
        }
      }
    }
  }
}
