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

        public void add(string connectionId, string contractId) {
            PutItemRequest request = new PutItemRequest
            {
                TableName = connectionTableName,
                Item = new Dictionary<string, AttributeValue>() {
                    {
                        "ConnectionId", new AttributeValue{ S = connectionId }
                    },
                    {
                        "ContractId", new AttributeValue{ S = contractId }
                    }
                }
            };

            var response = amazonDynamoDbClient.PutItemAsync(request);
            response.Wait();
        }

        public void delete(string connectionId) {
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = connectionTableName,
                Key = new Dictionary<string, AttributeValue>() {
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
