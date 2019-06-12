using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace AwsDotnetCsharp
{
    class Asset
    {
        private readonly AmazonDynamoDBClient amazonDynamoDbClient;
        private readonly string assetTableName = "Assets";

        public Asset()
        {
            amazonDynamoDbClient = new AmazonDynamoDBClient();
        }

        public Document getItem(string contractId) {
            var table = Table.LoadTable(amazonDynamoDbClient, assetTableName);
            var result = table.GetItemAsync(contractId);
            result.Wait();
            return result.Result;
        }
    }
}
