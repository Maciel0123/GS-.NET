using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WScoreApi.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            // marca como deprecated se a versão estiver obsoleta
            operation.Deprecated = apiDescription.IsDeprecated();

            // garante que todos os status codes documentados existam
            foreach (var responseType in apiDescription.SupportedResponseTypes)
            {
                var responseKey = responseType.StatusCode.ToString();

                if (!operation.Responses.ContainsKey(responseKey))
                {
                    operation.Responses[responseKey] = new OpenApiResponse
                    {
                        Description = "Resposta padrão"
                    };
                }
            }
        }
    }
}
