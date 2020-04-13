using System;
using POC.DynamoDB.Api.Models.Fixed;

namespace POC.DynamoDB.Api.Models.Entities
{
    public class Product : Entity
    {
        public Product()
        {
        }

        public Product(Guid id, string description, string manufacturer, ProductType productType,
            DateTime validityStart, DateTime validityEnd)
        {
            Id = id;
            Manufacturer = manufacturer;
            Description = description;
            Status = Status.Active;
            ProductType = productType;
            ValidityStart = validityStart;
            ValidityEnd = validityEnd;
            PartitionKey = manufacturer;
            SortKey = $"{validityStart.ToShortDateString()}#{validityEnd.ToShortDateString()}#{Id}";
        }

        public Product(string description, string manufacturer, ProductType productType, DateTime validityStart,
            DateTime validityEnd)
            : this(Guid.NewGuid(), description, manufacturer, productType, validityStart, validityEnd)
        {
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ValidityStart { get; }
        public DateTime ValidityEnd { get; }
        public Status Status { get; set; }
        public ProductType ProductType { get; set; }
        public sealed override string PartitionKey { get; set; }
        public sealed override string SortKey { get; set; }
    }
}