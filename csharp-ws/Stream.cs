using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;

namespace AwsDotnetCsharp
{
    class Stream
    {
        public void ProcessDynamoEvent(DynamoDBEvent dynamoDBEvent, ILambdaContext context)
        {
            AmazonApiGatewayManagementApiClient client = new AmazonApiGatewayManagementApiClient(new AmazonApiGatewayManagementApiConfig()
            {
                ServiceURL = "https://g49fepw5h8.execute-api.us-west-2.amazonaws.com/Test"
            });

            AmazonDynamoDBClient amazonDynamoDbClient = new AmazonDynamoDBClient();

            Console.WriteLine("Voy a empezar");
            Console.WriteLine(dynamoDBEvent.Records);

            foreach (var dynamoRecord in dynamoDBEvent.Records)
            {
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dynamoRecord.Dynamodb.NewImage)));
                String[] connections = GetConnections(amazonDynamoDbClient, dynamoRecord.Dynamodb.NewImage);
                foreach (string connection in connections)
                {
                    PostToConnectionRequest postRequest = new PostToConnectionRequest()
                    {
                        ConnectionId = connection,
                        Data = stream
                    };

                    var result = client.PostToConnectionAsync(postRequest);
                    result.Wait();
                }
            }
        }

        private string[] GetConnections(AmazonDynamoDBClient amazonDynamoDbClient, Dictionary<string, AttributeValue> newImage)
        {
            if (!ExistsContractId(newImage))
            {
                return new string[0];
            }

            var contractId = newImage["ContractId"].S;
            var request = new QueryRequest()
            {
                TableName = "Connection",
                KeyConditionExpression = "ContractId = :ContractId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                    {
                        ":ContractId", new AttributeValue(){  S = contractId }
                    }
                }
            };

            var response = amazonDynamoDbClient.QueryAsync(request);
            response.Wait();

            List<String> connections = new List<string>();
            foreach (var item in response.Result.Items)
            {
                connections.Add(item["ConnectionId"].S);
            }

            return connections.ToArray();
        }

        private bool ExistsContractId(Dictionary<string, AttributeValue> newImage)
        {
            try
            {
                var test = newImage["ContractId"].S;
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
