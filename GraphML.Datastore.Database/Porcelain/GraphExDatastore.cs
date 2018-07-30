using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using GraphML.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Datastore.Database.Porcelain
{
  public sealed class GraphExDatastore : DatastoreBase<GraphEx>, IGraphExDatastore
  {
    public GraphExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphExDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    // ids --> graph.Id
    public override IEnumerable<GraphEx> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        var graphs = _dbConnection.GetAll<Graph>().Where(x => ids.Contains(x.Id));
        var graphItemAttrs = _dbConnection.GetAll<GraphItemAttribute>().Where(x => ids.Contains(x.OwnerId));

        var nodes = _dbConnection.GetAll<Node>().Where(x => ids.Contains(x.OwnerId));
        var nodeIds = nodes.Select(x => x.Id);
        var nodeItemAttrs = _dbConnection.GetAll<NodeItemAttribute>().Where(x => nodeIds.Contains(x.OwnerId));

        var edges = _dbConnection.GetAll<Edge>().Where(x => ids.Contains(x.OwnerId));
        var edgeIds = edges.Select(x => x.Id);
        var edgeItemAttrs = _dbConnection.GetAll<EdgeItemAttribute>().Where(x => edgeIds.Contains(x.OwnerId));

        var retval = new GraphEx
        {
          Graphs = graphs,
          Nodes = nodes,
          Edges = edges,
          GraphItemAttributes = graphItemAttrs,
          NodeItemAttributes = nodeItemAttrs,
          EdgeItemAttributes = edgeItemAttrs
        };

        return new[] { retval };
      });
    }

    // ownerIds --> Repository.Id
    public override IEnumerable<GraphEx> ByOwners(IEnumerable<string> ownerIds)
    {
      return GetInternal(() =>
      {
        var graphs = _dbConnection.GetAll<Graph>().Where(x => ownerIds.Contains(x.OwnerId));
        var graphIds = graphs.Select(x => x.Id);

        return ByIds(graphIds);
      });
    }

    public override IEnumerable<GraphEx> Create(IEnumerable<GraphEx> entity)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          foreach (var ent in entity)
          {
            foreach (var graph in ent.Graphs) { graph.Id = graph.Id = graph.Id == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : graph.Id; }
            _dbConnection.Insert(ent.Graphs, trans);

            foreach (var graphAttr in ent.GraphItemAttributes) { graphAttr.Id = graphAttr.Id = graphAttr.Id == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : graphAttr.Id; }
            _dbConnection.Insert(ent.GraphItemAttributes, trans);

            foreach (var node in ent.Nodes) { node.Id = node.Id = node.Id == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : node.Id; }
            _dbConnection.Insert(ent.Nodes, trans);

            foreach (var nodeAttr in ent.NodeItemAttributes) { nodeAttr.Id = nodeAttr.Id = nodeAttr.Id == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : nodeAttr.Id; }
            _dbConnection.Insert(ent.NodeItemAttributes, trans);

            foreach (var edge in ent.Edges) { edge.Id = edge.Id = edge.Id == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : edge.Id; }
            _dbConnection.Insert(ent.Edges, trans);

            foreach (var edgeAttr in ent.EdgeItemAttributes) { edgeAttr.Id = edgeAttr.Id = edgeAttr.Id == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : edgeAttr.Id; }
            _dbConnection.Insert(ent.EdgeItemAttributes, trans);
          }
          trans.Commit();

          return entity;
        }
      });
    }

    public override void Delete(IEnumerable<GraphEx> entity)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          foreach (var ent in entity)
          {
            _dbConnection.Delete(ent.Graphs, trans);
            _dbConnection.Delete(ent.GraphItemAttributes, trans);
            _dbConnection.Delete(ent.Nodes, trans);
            _dbConnection.Delete(ent.NodeItemAttributes, trans);
            _dbConnection.Delete(ent.Edges, trans);
            _dbConnection.Delete(ent.EdgeItemAttributes, trans);
          }
          trans.Commit();

          return 0;
        }
      });
    }

    public override void Update(IEnumerable<GraphEx> entity)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          foreach (var ent in entity)
          {
            _dbConnection.Update(ent.Graphs, trans);
            _dbConnection.Update(ent.GraphItemAttributes, trans);
            _dbConnection.Update(ent.Nodes, trans);
            _dbConnection.Update(ent.NodeItemAttributes, trans);
            _dbConnection.Update(ent.Edges, trans);
            _dbConnection.Update(ent.EdgeItemAttributes, trans);
          }
          trans.Commit();

          return 0;
        }
      });
    }
  }
}
