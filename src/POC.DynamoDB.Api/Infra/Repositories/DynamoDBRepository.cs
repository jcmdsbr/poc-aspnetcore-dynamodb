using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using POC.DynamoDB.Api.Infra.Contracts;
using POC.DynamoDB.Api.Models.Entities;

namespace POC.DynamoDB.Api.Infra.Repositories
{
    public class DynamoDbRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        protected readonly DynamoDBContext Context;

        public DynamoDbRepository(IAmazonDynamoDB client)
        {
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
    }
}