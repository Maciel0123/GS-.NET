using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WScoreApi.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions
                    .FirstOrDefault(p => p.Name == parameter.Name);

                if (description == null)
                    continue;

                parameter.Description ??= description.ModelMetadata?.Description;

                if (parameter.Required == false)
                    parameter.Required = description.IsRequired;
            }

            operation.Summary ??= apiDescription.ActionDescriptor.DisplayName;
        }
    }
}
