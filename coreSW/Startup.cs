using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Rendertron;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace coreSW
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
            // Add rendertron services
            services.AddRendertron(options =>
            {
                // use http compression
                options.AcceptCompression = true;
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRendertron(new RendertronMiddlewareOptions()
            {
                ProxyUrl = "https://render-tron.appspot.com/render/",
                InjectShadyDom = true
            });

            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" });
            });
        }
    }
}
