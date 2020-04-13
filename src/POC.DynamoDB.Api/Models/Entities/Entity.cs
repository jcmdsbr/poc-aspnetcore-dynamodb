using Amazon.DynamoDBv2.DataModel;

namespace POC.DynamoDB.Api.Models.Entities
{
    [DynamoDBTable("PocDynamoApp")]
    public abstract class Entity
    {
        [DynamoDBHashKey] public abstract string PartitionKey { get; set; }

        [DynamoDBRangeKey] public abstract string SortKey { get; set; }

        [DynamoDBVersion] public int? NumberOfVersion { get; set; }
    }
}