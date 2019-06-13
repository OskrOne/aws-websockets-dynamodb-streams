using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace AwsDotnetCsharp
{
    class Connection
    {
        private readonly AmazonDynamoDBClient amazonDynamoDbClient;
        private readonly string connectionTableName = "Connection";

        public Connection()
        {
            amazonDynamoDbClient = new AmazonDynamoDBClient();
        }

        public void Add(string connectionId, string contractId) {
            PutItemRequest request = new PutItemRequest
            {
                TableName = connectionTableName,
                Item = new Dictionary<string, AttributeValue>() {
                    {
                        "ContractId", new AttributeValue{ S = contractId }
                    },
                    {
                        "ConnectionId", new AttributeValue{ S = connectionId }
                    }
                }
            };

            var response = amazonDynamoDbClient.PutItemAsync(request);
            response.Wait();
        }

        public void Delete(string connectionId) {

            ScanRequest scanRequest = new ScanRequest()
            {
                TableName = "Connection",
                FilterExpression = "ConnectionId = :ConnectionId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                    {
                        ":ConnectionId", new AttributeValue(){ S = connectionId}
                    }
                }
            };

            var scanResponse = amazonDynamoDbClient.ScanAsync(scanRequest);
            scanResponse.Wait();

            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = connectionTableName,
                Key = new Dictionary<string, AttributeValue>() {
                    {
                        "ContractId", new AttributeValue{ S = scanResponse.Result.Items[0]["ContractId"].S }
                    },
                    {
                        "ConnectionId", new AttributeValue{ S = connectionId }
                    }
                }

            };

            var response = amazonDynamoDbClient.DeleteItemAsync(request);
            response.Wait();
        }
    }

}
