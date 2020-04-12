using System;
using POC.DynamoDB.Api.Models.Fixed;

namespace POC.DynamoDB.Api.Models.Request
{
    public class ProductFilterRequest
    {
        public string Manufacturer { get; set; }

        public ProductType ProductType { get; set; }

        public string Description { get; set; }

        public bool IncludedInactive { get; set; }

        public DateTime ValidityStart { get; set; } = DateTime.MinValue;

        public DateTime ValidityEnd { get; set; } = DateTime.MaxValue;

        public int Take { get; set; } = 10;

        public int Skip { get; set; }
    }
}