using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Asp.Versioning.ApiExplorer;   
using Microsoft.Extensions.Configuration;


namespace WScoreApi.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _cfg;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration cfg)
        {
            _provider = provider;
            _cfg = cfg;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Title = _cfg["Swagger:Title"] ?? "WScore API",
                    Version = desc.ApiVersion.ToString(),
                    Description = _cfg["Swagger:Description"] ?? "API de bem-estar e futuro do trabalho (WScore)"
                });
            }

            // XML comments (comentários de summary nos controllers)
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        }
    }
}
