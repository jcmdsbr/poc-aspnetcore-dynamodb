using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using POC.DynamoDB.Api.Models.Entities;
using POC.DynamoDB.Api.Models.Request;

namespace POC.DynamoDB.Api.Infra.Contracts
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> FindAsync(ProductFilterRequest filters, CancellationToken cancellationToken);
    }
}