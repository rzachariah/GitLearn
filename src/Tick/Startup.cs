using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Tick.Cache;
namespace HelloWorld
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940


        public void Configure(IApplicationBuilder app )
        {
            app.UseMvc();
	        app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!!.\nThis is a ASP.NET vNext test Application on Docker.\n\nMaintained by Ashish Sharma\nEze Software Group");
                
            });
        }
        
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<ISymbolCache, SymbolCache>();
            services.AddSingleton<IPositionCache, PositionCache>();
            services.AddSingleton<IPriceCache, PriceCache>();
        }
    }
}
