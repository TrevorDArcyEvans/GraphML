using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GraphML.API
{
  public sealed class ApiModelDocumentFilter<T> : IDocumentFilter where T : class
  {
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
      context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
    }
  }
}
