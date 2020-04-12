using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using POC.DynamoDB.Api.Infra.Contracts;
using POC.DynamoDB.Api.Models.Entities;

namespace POC.DynamoDB.Api.Infra.Repositories
{
    public class DynamoDbRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        private readonly IAmazonDynamoDB _client;
        protected readonly DynamoDBContext Context;

        public DynamoDbRepository(IAmazonDynamoDB client)
        {
            _client = client;
            Context = new DynamoDBContext(client);
        }

        public async Task SaveOrUpdate(TEntity obj, CancellationToken cancellationToken)
        {
            await Context.SaveAsync(obj, cancellationToken);
        }

        public async Task RemoveAsync(object key, CancellationToken cancellationToken)
        {
            await Context.DeleteAsync<TEntity>(key, cancellationToken);
        }

        public async Task GetByKey(object key, CancellationToken cancellationToken)
        {
            await Context.LoadAsync<TEntity>(key, cancellationToken);
        }

        public async Task<CreateTableResponse> SetupAsync(CancellationToken cancellationToken)
        {
            var createTableRequest = new CreateTableRequest
            {
                TableName = "PocDynamoApp",
                AttributeDefinitions = new List<AttributeDefinition>(),
                KeySchema = new List<KeySchemaElement>(),
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>(),
                LocalSecondaryIndexes = new List<LocalSecondaryIndex>(),
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };
            createTableRequest.KeySchema = new[]
            {
                new KeySchemaElement
                {
                    AttributeName = nameof(Entity.PartitionKey),
                    KeyType = KeyType.HASH
                }
            }.ToList();

            createTableRequest.AttributeDefinitions = new[]
            {
                new AttributeDefinition
                {
                    AttributeName = nameof(Entity.SortKey),
                    AttributeType = ScalarAttributeType.N
                }
            }.ToList();

            return await _client.CreateTableAsync(createTableRequest, cancellationToken);
        }
    }
}