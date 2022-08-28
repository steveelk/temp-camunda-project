
using LAFDalApp;
using LAFDalApp.Admin;
using LAFWebApp.Bpmn;
using Microsoft.AspNetCore.Identity;

namespace LAFWebApp
{
    public class Startup
    {
        private const string camundaRestApiUri = "http://localhost:8080/engine-rest";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //services.AddCamunda(camundaRestApiUri);
        }

        //public void Configure(WebApplication app, IWebHostEnvironment env)
        //{
        //    if (!app.Environment.IsDevelopment())
        //    {
        //        app.UseExceptionHandler("/Error");
        //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //        app.UseHsts();
        //    }
        //    app.UseHttpsRedirection();
        //    app.UseStaticFiles();
        //    app.UseRouting();
        //    app.UseAuthorization();
        //    app.MapRazorPages();
        //    app.Run();
        //}
    }
}
