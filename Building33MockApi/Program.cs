using StackExchange.Redis;

namespace Building33MockApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IDatabase>(cfg =>
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS_HOST"));
                return redis.GetDatabase();
            });

            builder.Services.AddGrpc().AddJsonTranscoding();
            builder.Services.AddGrpcReflection();
            builder.Services.AddGrpcSwagger();
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

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}
            app.UseCors("AllowAll");
            app.MapGrpcService<GrpcMessageService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapSwagger();
            app.MapGrpcReflectionService();
            app.MapRazorPages();

            app.Run();
        }
    }
}
