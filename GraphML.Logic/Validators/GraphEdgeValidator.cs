﻿using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphEdgeValidator : OwnedValidatorBase<GraphEdge>, IGraphEdgeValidator
  {
    public GraphEdgeValidator(
    IHttpContextAccessor context,
    IContactDatastore contactDatastore,
    IRoleDatastore roleDatastore) :
    base(context, contactDatastore, roleDatastore)
    {
      RuleSet(nameof(IGraphEdgeLogic.ByNodeIds), () =>
      {
      });
    }
  }
}