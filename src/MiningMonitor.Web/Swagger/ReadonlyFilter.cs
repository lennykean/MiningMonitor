using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiningMonitor.Web.Swagger
{
    public class ReadonlyFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
                return;

            foreach (var property in schema.Properties.Values)
            {
                property.ReadOnly = null;
            }
        }
    }
}