using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiningMonitor.Web.Swagger
{
    internal class LowercaseDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.Paths.ToList())
            {
                swaggerDoc.Paths.Remove(path);
                swaggerDoc.Paths.Add(path.Key.ToLower(), path.Value);
            }
        }
    }
}