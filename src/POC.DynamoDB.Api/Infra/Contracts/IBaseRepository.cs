using System.Threading;
using System.Threading.Tasks;
using POC.DynamoDB.Api.Models.Entities;

namespace POC.DynamoDB.Api.Infra.Contracts
{
    public interface IBaseRepository<in TEntity> where TEntity : Entity
    {
        Task SaveOrUpdate(TEntity obj, CancellationToken cancellationToken = default);
        Task RemoveAsync(object key, CancellationToken cancellationToken = default);
        Task GetByKey(object key, CancellationToken cancellationToken = default);
    }
}