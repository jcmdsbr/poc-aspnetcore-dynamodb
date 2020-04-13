using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using POC.DynamoDB.Api.Infra.Contracts;
using POC.DynamoDB.Api.Infra.Repositories;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace POC.DynamoDB.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services
                .AddSwaggerGen(c =>
                {
                    c.UseInlineDefinitionsForEnums();
                    c.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = "POC DynamoDB",
                            Version = "v1",
                            Description = ""
                        });
                    c.MapType<Guid>(() => new OpenApiSchema {Type = "string", Format = "uuid"});
                });

            services.AddScoped(typeof(IBaseRepository<>), typeof(DynamoDbRepository<>));

            services.AddScoped<IProductRepository, ProductRepository>();

            var config = new AmazonDynamoDBConfig
                {RegionEndpoint = RegionEndpoint.SAEast1, ServiceURL = Configuration["AWS:ConnectionString"]};
            var credentials = new BasicAWSCredentials(Configuration["AWS:AccessKey"], Configuration["AWS:SecretKey"]);
            services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(credentials, config));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("v1/swagger.json", "POC.DynamoDB.Api v1.0");
                s.EnableFilter();
                s.RoutePrefix = "swagger";
                s.DocExpansion(DocExpansion.None);
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}