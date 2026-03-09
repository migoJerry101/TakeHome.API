using Asp.Versioning;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace TakeHome.API.Extensions
{
    public static class AddSwaggerGenExtensions
    {
        public static IServiceCollection AddSwaggerGenWithOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Packaging API", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Packaging API - V2", Version = "v2" });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                    var versions = methodInfo.DeclaringType?
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions?.Any(v => $"v{v.MajorVersion}" == docName) ?? false;
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter  your valid token"
                });

                c.AddSecurityRequirement(document =>
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                        });
            });
            return services;
        }
    }
}
