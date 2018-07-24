using System.Data;

namespace GraphML.Datastore.Database.Interfaces
{
  public interface IDbConnectionFactory
  {
    IDbConnection Get();
  }
}
