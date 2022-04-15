using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProCare.API.Core.Constants;
using ProCare.Common;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProCare.API.Common
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
            ApplicationSettings.LoadSettings(Configuration);

            string instrumentationKey = getApplicationInsightsInstrumentationKey();

            if(!string.IsNullOrEmpty(instrumentationKey))
            {
                services.AddApplicationInsightsTelemetry(instrumentationKey);
            }

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost()
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
                using (SqlConnection connection = new SqlConnection(ApplicationSettings.ConfigurationsConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("usp_GetSettings", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SettingKey", ConfigSetttingKey.ApplicationInsightsInstrumentationKeys);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {

                            string settingValue = reader["SettingValue"].ToString();

                            if (Convert.ToBoolean(reader["IsEncrypted"]))
                            {
                                output = JsonConfigReader.Deserialize<Dictionary<string, string>>(EncryptionHelper.RijndaelDecryptString(settingValue, ApplicationSettings.ConfigurationsEncryptionKey))[ConfigSetttingKey.InstrumentationKey_CommonAPI];
                            }
                            else
                            {
                                output = JsonConfigReader.Deserialize<Dictionary<string, string>>(settingValue)[ConfigSetttingKey.InstrumentationKey_CommonAPI];
                            }
                        }
                    }
                }                
            }
            catch { }

            return output;
        }

        #endregion


    }
}
