using FlexiPay.Repositories;
using FlexiPay.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Register Redis connection
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            // This ensures that the connection is long-lived.
            return ConnectionMultiplexer.Connect("localhost:6379");
        });

        services.AddScoped<IRedisRepository, RedisRepository>();

        // Stripe Secret Key from configuration or environment variable
        var stripeSecretKey = Configuration["Stripe:SecretKey"];

        // Register StripeRepository and StripeService for Dependency Injection
        services.AddSingleton<IStripeRepository>(new StripeRepository(stripeSecretKey));
        services.AddScoped<IStripeService, StripeService>();

        // Add controllers and other necessary services
        services.AddControllers();

        // Register the Swagger generator, defining one or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FlexiPay API",
                Version = "v1",
                Description = "A flexible payment platform API"
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlexiPay API v1");
                c.RoutePrefix = "swagger";  // Makes the Swagger UI available at the app's root
            });
        }

        app.UseHsts();

        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlexiPay API v1");
            c.RoutePrefix = "swagger";  // Makes the Swagger UI available at the app's root
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
