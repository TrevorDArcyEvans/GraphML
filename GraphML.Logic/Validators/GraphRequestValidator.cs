using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  // IGraphRequestValidator --> requester is same org as graph
  //    IGraphNodeDatastore.ByIds --> GraphNode.GraphId
  //    IGraphDatastore.ByIds --> Graph.RepositoryId
  //    IRepositoryDatastore.ByIds --> Repository.RepositoryManagerId
  //    IRepositoryManagerDatastore.ByIds --> RepositoruManager.OrganisationId
  //    IOrganisationDatastore.ByIds --> Organisation.Id
  //    IContactDatastore.ByEmail --> Contact.OrganisationId
  public sealed class GraphRequestValidator : ValidatorBase<Graph>, IGraphRequestValidator
  {
    public GraphRequestValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
