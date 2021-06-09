using System.Collections.Generic;
using System.Linq;
using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Phonix;

namespace GraphML.Analysis.FindDuplicates
{
  public sealed class FindDuplicatesJob : IFindDuplicatesJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<FindDuplicatesJob> _logger;
    private readonly IGraphNodeDatastore _nodeDatastore;
    private readonly IResultLogic _resultLogic;

    public FindDuplicatesJob(
      IConfiguration config,
      ILogger<FindDuplicatesJob> logger,
      IGraphNodeDatastore nodeDatastore,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _nodeDatastore = nodeDatastore;
      _resultLogic = resultLogic;
    }

    public void Run(IRequest req)
    {
      const int MaxKeyLength = 15;

      var findDupesReq = (IFindDuplicatesRequest) req;
      var graphId = findDupesReq.GraphId;

      // raw nodes from db
      var nodes = _nodeDatastore.ByOwners(new[] { graphId }, 0, int.MaxValue, null).Items;
      var keys = GetKeys(nodes, MaxKeyLength);
      var dupes = GetDuplicates(keys);
      var result = new FindDuplicatesResult(dupes);
      
      var options = new JsonSerializerSettings();
      options.Converters.Add(new LookupSerializer<string[]>());
      var resultJson = JsonConvert.SerializeObject(result, options);
      
      // TODO   JSON serialisation
      List<IGrouping<string, string[]>> filteredDupes = dupes
        .Where(x => x.Count() > 1 && x.Key.Length > findDupesReq.MinMatchingKeyLength)
        .ToList();
      var json = JsonConvert.SerializeObject(filteredDupes, options);

      _resultLogic.Create(req, resultJson);
    }

    // [ metaphone-key ] --> [ duplicate-GraphNode.Id[] ]
    private static LookupEx<string, string[]> GetDuplicates(LookupEx<string, string[]> results)
    {
      var dupes = new LookupEx<string, string[]>();
      foreach (var grp in results)
      {
        foreach (var keys in grp)
        {
          keys.ToList().ForEach(key =>
          {
            var grping = dupes.GetGrouping(key, true);
            grping.Add(new[] { grp.Key });
          });
        }
      }

      return dupes;
    }

    // [ GraphNode.Id ] --> [ metaphone-keys[] ]
    private static LookupEx<string, string[]> GetKeys(IEnumerable<GraphNode> nodes, int maxLength)
    {
      var generator = new DoubleMetaphone(maxLength);
      var retval = new LookupEx<string, string[]>();
      foreach (var node in nodes)
      {
        var grping = retval.GetGrouping(node.Id.ToString(), true);
        var keys = generator.BuildKeys(node.Name);
        grping.Add(keys);
      }

      return retval;
    }
  }
}
