﻿using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class RepositoryItemAttributeDatastore : OwnedItemDatastoreBase<RepositoryItemAttribute>, IRepositoryItemAttributeDatastore
  {
    public RepositoryItemAttributeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<RepositoryItemAttributeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
