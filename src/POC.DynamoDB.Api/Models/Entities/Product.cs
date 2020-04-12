using System;
using POC.DynamoDB.Api.Models.Fixed;
using POC.DynamoDB.Api.Models.ValueObjects;

namespace POC.DynamoDB.Api.Models.Entities
{
    public class Product : Entity
    {
        public Product(string description, string manufacturer, ProductType productType, Validity validity)
        {
            Id = Guid.NewGuid();
            Manufacturer = manufacturer;
            Description = description;
            Status = Status.Active;
            ProductType = productType;
            Validity = validity;
            PartitionKey = manufacturer;
            SortKey = $"{validity.Start.ToShortDateString()}#{validity.End.ToShortDateString()}#{Id}#{Status}";
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public Validity Validity { get; set; }
        public Status Status { get; set; }
        public ProductType ProductType { get; set; }
        public sealed override string PartitionKey { get; set; }
        public sealed override string SortKey { get; set; }
    }
}