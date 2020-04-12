using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC.DynamoDB.Api.Infra.Repositories;
using POC.DynamoDB.Api.Models.Entities;

namespace POC.DynamoDB.Api.Background
{
    public class SeedBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var provider = _serviceProvider.GetRequiredService(typeof(IAmazonDynamoDB)) as IAmazonDynamoDB;
            var dynamoDb = new DynamoDbRepository<Entity>(provider);
            await dynamoDb.SetupAsync(stoppingToken);
        }
    }
}