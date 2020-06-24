using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ExporterWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
#if RELEASE
            FixtureUtils.PopulateDatabase(host);
#endif
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
