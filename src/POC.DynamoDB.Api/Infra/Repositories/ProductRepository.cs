using System.Collections.Generic;
using System.Linq;
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

            if (!string.IsNullOrWhiteSpace(filters.Description))
                opConfig.QueryFilter.Add(new ScanCondition(nameof(Product.Description), ScanOperator.Contains,
                    filters.Description));

            if (filters.ProductType != ProductType.None)
                opConfig.QueryFilter.Add(new ScanCondition(nameof(Product.ProductType), ScanOperator.Contains,
                    filters.ProductType));


            if (!filters.IncludedInactive)
                opConfig.QueryFilter.Add(new ScanCondition(nameof(Product.Status), ScanOperator.Equal, Status.Active));


            var sortKey = new List<string>();

            if (filters.ValidityStart.HasValue)
                sortKey.Add(filters.ValidityStart.Value.ToShortDateString());

            if (filters.ValidityEnd.HasValue)
                sortKey.Add(filters.ValidityEnd.Value.ToShortDateString());


            // When partition key exists !
            if (!string.IsNullOrWhiteSpace(filters.Manufacturer))
            {
                // query filters use in a filter by composite key
                var queryFilter = new QueryFilter();

                queryFilter.AddCondition(nameof(Product.PartitionKey), QueryOperator.Equal, filters.Manufacturer);

                if (sortKey.Any())
                    queryFilter.AddCondition(nameof(Product.SortKey), QueryOperator.BeginsWith,
                        string.Join("#", sortKey));

                // Use scan condition and query filter (filter by primary key)
                return await Context.FromQueryAsync<Product>(new QueryOperationConfig
                {
                    Filter = queryFilter,
                    Limit = filters.Take
                }, opConfig).GetRemainingAsync(cancellationToken);
            }

            // Table scan !! scan condition shouldn't contain a primary key
            var scanFilter = new ScanFilter();

            scanFilter.AddCondition(nameof(Product.SortKey), ScanOperator.BeginsWith, string.Join("#", sortKey));

            return await Context.FromScanAsync<Product>(new ScanOperationConfig
            {
                Limit = 10
            }, opConfig).GetRemainingAsync(cancellationToken);
        }
    }
}