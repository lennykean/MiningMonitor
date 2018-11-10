using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

using System.Linq;

namespace MiningMonitor.Web.Swagger
{
    internal class FromClaimParameterFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var claimParameter = operation.Parameters.FirstOrDefault(p => p.Name.StartsWith("http://schemas.xmlsoap.org/ws/2005/05/identity/claims"));
            if (claimParameter != null)
                operation.Parameters.Remove(claimParameter);
        }
    }
}