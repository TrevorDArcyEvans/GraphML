using System.Collections.Generic;
using System.Linq;
using GraphML.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GraphML.API
{
  public sealed class AuthorizeCheckOperationFilter : IOperationFilter
  {
    private readonly string[] _scopes;

    public AuthorizeCheckOperationFilter(IConfiguration config)
    {
      _scopes = config.IDENTITY_SERVER_SCOPES().ToArray();
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                         context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

      if (hasAuthorize)
      {
        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

        operation.Security = new List<OpenApiSecurityRequirement>
        {
          new OpenApiSecurityRequirement
          {
              [
                new OpenApiSecurityScheme
                {
                  Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                  }
                }
              ] = _scopes
          }
        };
      }
    }
  }
}
