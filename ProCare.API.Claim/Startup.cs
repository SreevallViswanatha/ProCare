using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProCare.API.Core;
using ProCare.API.Core.Constants;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ProCare.API.Claims
{
    public class Startup
    {
        #region Public Variables

        public IConfiguration Configuration { get; }

        #endregion Public Variables

        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Licensing.RegisterLicense(Global.ServiceStackLicense);
            Configuration = configuration;
        }

        #endregion Constructors

        #region Public Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                ApplicationSettings.LoadSettings(Configuration);

                string instrumentationKey = getApplicationInsightsInstrumentationKey();

                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    services.AddApplicationInsightsTelemetry(instrumentationKey);
                }

                services.Configure<IISServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("");
                Debug.WriteLine("<<<<<<<<<<<<<<<<<<<<   CLAIM API STARTUP ERROR   >>>>>>>>>>>>>>>>>>>>>");
                Debug.WriteLine(ex);
                Debug.WriteLine("<<<<<<<<<<<<<<<<<<<< END CLAIM API STARTUP ERROR >>>>>>>>>>>>>>>>>>>>>");
                Debug.WriteLine("");
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }

        #endregion Public Methods

        #region Private Methods

        private string getApplicationInsightsInstrumentationKey()
        {
            string output = null;

            try
            {
                CommonApiHelper commonApiHelper = new CommonApiHelper(ApplicationSettings.CommonApiBaseUrl, ApplicationSettings.CommonApiUsername, ApplicationSettings.CommonApiPassword);

                Task<Dictionary<string, string>> t = commonApiHelper.GetSetting(ConfigSetttingKey.ApplicationInsightsInstrumentationKeys);
                t.Wait(ApplicationSettings.GetCancellationToken());
                Dictionary<string, string> results = t.Result;
                output = results[ConfigSetttingKey.InstrumentationKey_ClaimsAPI];
            }
            catch { }

            return output;
        }

        #endregion
    }
}
