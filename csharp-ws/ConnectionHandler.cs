using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Net;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class ConnectionHandler
    {
        public APIGatewayProxyResponse Connect(APIGatewayProxyRequest request)
        {
            if (!queryStringValid(request.QueryStringParameters))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = "Invalid query string, you must include ContractId"
                };
            }

            Connection connection = new Connection();
            String contractId = request.QueryStringParameters["ContractId"];
            connection.Add(request.RequestContext.ConnectionId, contractId);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        private bool queryStringValid(IDictionary<string, string> queryString)
        {
            try
            {
                var value = queryString["ContractId"];
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public APIGatewayProxyResponse Disconnect(APIGatewayProxyRequest request)
        {
            Connection connection = new Connection();
            connection.Delete(request.RequestContext.ConnectionId);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
