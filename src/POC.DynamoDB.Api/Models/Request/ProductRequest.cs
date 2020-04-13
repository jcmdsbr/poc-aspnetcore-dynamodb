using System;
using Newtonsoft.Json;
using POC.DynamoDB.Api.Models.Entities;
using POC.DynamoDB.Api.Models.Fixed;

namespace POC.DynamoDB.Api.Models.Request
{
    public class ProductRequest
    {
        [JsonIgnore] public Guid Id { get; set; }

        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public ProductType ProductType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public static explicit operator Product(ProductRequest request)
        {
            return new Product(request.Description, request.Manufacturer, request.ProductType, request.Start,
                request.End);
        }
    }
}