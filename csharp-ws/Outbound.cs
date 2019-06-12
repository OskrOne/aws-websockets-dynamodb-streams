using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using static Amazon.Lambda.APIGatewayEvents.APIGatewayProxyRequest;

namespace AwsDotnetCsharp
{
    class Outbound
    {
        public static void SendPosition(ProxyRequestContext context, string contractId)
        {
            Asset asset = new Asset();
            Document document = asset.getItem(contractId);
            AmazonApiGatewayManagementApiClient client = new AmazonApiGatewayManagementApiClient(new AmazonApiGatewayManagementApiConfig()
            {
                ServiceURL = "https://" + context.DomainName + "/" + context.Stage
            });

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(document)));
            PostToConnectionRequest postRequest = new PostToConnectionRequest()
            {
                ConnectionId = context.ConnectionId,
                Data = stream
            };

            var result = client.PostToConnectionAsync(postRequest);
            result.Wait();
        }
    }
}
