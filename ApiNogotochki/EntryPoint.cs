using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

#nullable enable

namespace ApiNogotochki
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<ApiNogotochkiStartup>();
                });
    }
}