using ApiNogotochki.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiNogotochki
{
    public class ApiNogotochkiStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Scan(x => x.FromAssemblyOf<ApiNogotochkiStartup>()
                                .AddClasses(publicOnly: true)
                                .AsSelf().AsImplementedInterfaces()
                                .WithSingletonLifetime());
            services.AddControllers();
            services.AddCors();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {		
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            
            app.UseCors(builder => 
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            
            app.UseDbUserAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}