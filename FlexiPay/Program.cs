using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FlexiPay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Set up Kestrel with HTTPS
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenAnyIP(5001, listenOptions =>
                        {
                            listenOptions.UseHttps(); // HTTPS Port
                        });
                        // Configure HTTPS settings
                        options.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        });
                    });

                    // Use the Startup class to configure services and middleware
                    webBuilder.UseStartup<Startup>();
                });
    }
}
