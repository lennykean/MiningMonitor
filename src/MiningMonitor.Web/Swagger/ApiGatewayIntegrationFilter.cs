using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiningMonitor.Web.Swagger
{
    internal class ApiGatewayIntegrationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Extensions.Add("x-amazon-apigateway-integration", new
            {
                type = "aws_proxy",
                httpMethod = "POST",
                uri = new Dictionary<string, string>
                {
                    ["Fn::Sub"] = "arn:aws:apigateway:${{AWS::Region}}:lambda:path/2015-03-31/functions/${{MiningMonitorLambda.Arn}}/invocations"
                }
            });
        }
    }
}