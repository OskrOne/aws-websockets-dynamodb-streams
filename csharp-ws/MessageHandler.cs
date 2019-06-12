using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;
using System.Net;

namespace AwsDotnetCsharp
{
    class MessageHandler
    {
        public APIGatewayProxyResponse Message(APIGatewayProxyRequest request)
        {
            Console.WriteLine("ConnectionId: " + request.RequestContext.ConnectionId);
            Body body = JsonConvert.DeserializeObject<Body>(request.Body);
            Outbound.SendPosition(request.RequestContext, body.ContractId);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        public APIGatewayProxyResponse FreezeMoney(APIGatewayProxyRequest request) {
            Console.WriteLine("Freeze Money ConnectionId: " + request.RequestContext.ConnectionId);
            Body body = JsonConvert.DeserializeObject<Body>(request.Body);
            Inbound.FreezeMoney(body.ContractId);
            Outbound.SendPosition(request.RequestContext, body.ContractId);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }

    public class Body
    {
        public string ContractId { get; set; }
        public Body(string contractId)
        {
            ContractId = contractId;
        }

        public override string ToString()
        {
            return "ContractId: " + ContractId;
        }
    }
}
