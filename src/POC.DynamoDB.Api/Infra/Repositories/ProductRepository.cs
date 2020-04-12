using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using POC.DynamoDB.Api.Infra.Contracts;
using POC.DynamoDB.Api.Models.Entities;
using POC.DynamoDB.Api.Models.Fixed;
using POC.DynamoDB.Api.Models.Request;

namespace POC.DynamoDB.Api.Infra.Repositories
{
    public class ProductRepository : DynamoDbRepository<Product>, IProductRepository
    {
        public ProductRepository(IAmazonDynamoDB client) : base(client)
        {
        }

        public async Task<List<Product>> FindAsync(ProductFilterRequest filters, CancellationToken cancellationToken)
        {
            var includedInactive = filters.IncludedInactive ? "" : Status.Active.ToString();

            // Create a filter base a scan condition
            var opConfig = new DynamoDBOperationConfig {QueryFilter = new List<ScanCondition>()};

            opConfig.QueryFilter.Add(new ScanCondition(nameof(Product.Description), ScanOperator.Contains,
                filters.Description));

            opConfig.QueryFilter.Add(new ScanCondition(nameof(Product.ProductType), ScanOperator.Contains,
                filters.ProductType));

            // When partition key exists !
            if (!string.IsNullOrWhiteSpace(filters.Manufacturer))
            {
                // query filters use in a filter by composite key
                var queryFilter = new QueryFilter();

                queryFilter.AddCondition(nameof(Product.PartitionKey), QueryOperator.Equal, filters.Manufacturer);

                queryFilter.AddCondition(nameof(Product.SortKey), QueryOperator.Between,
                    filters.ValidityStart.ToShortDateString(), filters.ValidityEnd.ToShortDateString(),
                    includedInactive);

                // Use scan condition and query filter (filter by composite key)
                return await Context.FromQueryAsync<Product>(new QueryOperationConfig
                {
                    Filter = queryFilter,
                    Limit = filters.Take
                }, opConfig).GetRemainingAsync(cancellationToken);
            }

            // Table scan !! scan condition shouldn't contain a composite key

            // query filters use in a filter by composite key
            var scanFilter = new ScanFilter();

            scanFilter.AddCondition(nameof(Product.SortKey), ScanOperator.Contains,
                filters.ValidityStart.ToShortDateString(), filters.ValidityEnd.ToShortDateString(), includedInactive);

            return await Context.FromScanAsync<Product>(new ScanOperationConfig
            {
                Limit = 10
            }, opConfig).GetRemainingAsync(cancellationToken);
        }
    }
}