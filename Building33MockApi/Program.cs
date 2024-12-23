using Building33MockApi.Services;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Serilog.Sinks;
using Serilog;
using StackExchange.Redis;

namespace Building33MockApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();

            //builder.Host.UseSerilog((context, configuration) =>
            //{
            //    var httpAccessor = context.Configuration.Get<HttpContextAccessor>();
            //    configuration.ReadFrom.Configuration(context.Configuration)
            //                 .Enrich.WithEcsHttpContext(httpAccessor)
            //                 .Enrich.WithEnvironmentName()
            //                 .WriteTo.ElasticCloud(context.Configuration["ElasticCloud:CloudId"], context.Configuration["ElasticCloud:CloudUser"], context.Configuration["ElasticCloud:CloudPass"], opts =>
            //                 {
            //                     opts.DataStream = new Elastic.Ingest.Elasticsearch.DataStreams.DataStreamName("gateway-building33mockapi-new-logs");
            //                     opts.BootstrapMethod = BootstrapMethod.Failure;
            //                 });
            //});

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            var connectionString = Environment.GetEnvironmentVariable("REDIS_HOST");
            if(String.IsNullOrEmpty(connectionString))
            {
                connectionString = builder.Configuration.GetConnectionString("RedisConnection");
            }

            if (String.IsNullOrEmpty(connectionString))
            {
                throw new Exception("The connection string for Redis is empty!!!");
            }

            builder.Services.AddSingleton<IRedisDbProvider>(provider =>
            {
                return new RedisDbProvider(connectionString);
            });
            builder.Services.AddSingleton<ICacheHandler, RedisCacheHandler>();


            builder.Services.AddScoped<MessageService>();

            builder.Services.AddGrpc().AddJsonTranscoding();
            builder.Services.AddGrpcReflection();
            builder.Services.AddGrpcSwagger();
            builder.Services.AddControllers();
            builder.Services.AddRazorPages();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Building 33 Mock Api", Version = "v1" });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
            app.UseCors("AllowAll");
            app.MapGrpcService<GrpcMessageService>();
            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapSwagger();
            app.MapGrpcReflectionService();
            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }
}
