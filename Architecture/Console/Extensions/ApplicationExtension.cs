using Azure_Connection_Sample.Architecture.Domain_Layer.Models;
using Azure_Connection_Sample.Architecture.Service_Layer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Connection_Sample.Architecture.Console.Extensions
{
    internal static class ApplicationExtension
    {
        public static void Build(this ConfigurationManager manager, string configuration) => manager
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configuration, false, true)
            .AddUserSecrets<Program>(false, true)
            .AddEnvironmentVariables()
            .Build();

        public static void RegisterLogger(this IHostBuilder host)
        {
            host.UseSerilog((context, configuration) => configuration
            .MinimumLevel.Information()
            .WriteTo.Console());

            BuildStaticSerilog();
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddLogging(logger => logger.AddSerilog());
            services.AddHttpClient<IApiFacade, ApiFacade>();
            services.AddSingleton<IApiFacadeFactory, ApiFacadeFactory>();

            services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
            services.RemoveAll<IHttpMessageHandlerFactory>();
        }

        #region Private:

        private static void BuildStaticSerilog() => Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        #endregion

    }
}
