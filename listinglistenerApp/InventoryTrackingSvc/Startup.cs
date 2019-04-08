using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryTrackingSvc.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InventoryTrackingSvc
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();

      Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      configureInventoryAnalytics(services, this.Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();
    }

    private void configureInventoryAnalytics(IServiceCollection services, IConfiguration configuration)
    {
      string etsyBaseURL = configuration.GetValue<string>("Etsy:BaseURL");
      string etsyAPIKey = configuration.GetValue<string>("Etsy:APIKey");
      string dbCachePath = configuration.GetValue<string>("DBCache:Path");

      EtsyAPIProxy etsyAPI = new EtsyAPIProxy(etsyBaseURL, etsyAPIKey);
      ISyncDatabase db = new SyncDatabaseFS(dbCachePath);

      InventoryAnalytics ia = new InventoryAnalytics(db, etsyAPI);

      services.AddSingleton<InventoryAnalytics>(ia);
    }

  }

}
