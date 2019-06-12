using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace AwsDotnetCsharp
{
    class Inbound
    {
        private static readonly AmazonDynamoDBClient amazonDynamoDbClient = new AmazonDynamoDBClient();
        private static readonly string assetTableName = "Assets";

        public static void FreezeMoney(string contractId) {
            UpdateItemRequest request = new UpdateItemRequest()
            {
                TableName = assetTableName,
                Key = new Dictionary<string, AttributeValue>() {
                    {
                        "ContractId", new AttributeValue{ S = contractId }
                    }
                },
                ExpressionAttributeNames = new Dictionary<string, string>() {
                    { "#InstrumentId", "InstrumentId"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                    {
                        ":incr", new AttributeValue{ N = "1" }
                    }
                },
                UpdateExpression = "SET #InstrumentId = #InstrumentId + :incr"
            };

            var response = amazonDynamoDbClient.UpdateItemAsync(request);
            response.Wait();
        }
    }
}
