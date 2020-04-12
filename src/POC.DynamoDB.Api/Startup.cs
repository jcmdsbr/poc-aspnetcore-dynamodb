using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC.DynamoDB.Api.Background;
using POC.DynamoDB.Api.Infra.Contracts;
using POC.DynamoDB.Api.Infra.Repositories;

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

            services.AddScoped(typeof(IBaseRepository<>), typeof(DynamoDbRepository<>));

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddHostedService<SeedBackgroundService>();

            services.AddDefaultAWSOptions(SetAwsCredentials(Configuration.GetAWSOptions()));

            services.AddScoped<IAmazonDynamoDB>(x => new AmazonDynamoDBClient(new AmazonDynamoDBConfig
            {
                ServiceURL = "http://localhost:4569",
                UseHttp = true
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public AWSOptions SetAwsCredentials(AWSOptions awsOptions)
        {
            awsOptions.Credentials =
                new BasicAWSCredentials(Configuration["AWS:AccessKey"], Configuration["AWS:SecretKey"]);

            return awsOptions;
        }
    }
}