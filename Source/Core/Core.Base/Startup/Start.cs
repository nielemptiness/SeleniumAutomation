using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Serilog;

namespace Core.Base.Startup
{
    public class Start
    {
        public static void CreateConfiguration(string[] arg, string service, out string environment, out IConfiguration config)
        {
            var initialConfigBuilder = new ConfigurationBuilder().AddEnvironmentVariables("ASPNETCORE_");
            
            if (arg != null)
                initialConfigBuilder.AddCommandLine(arg);

            var initialConfig = (IConfiguration)initialConfigBuilder.Build();

            environment = string.IsNullOrEmpty(initialConfig[HostDefaults.EnvironmentKey])
                ? Environment.GetEnvironmentVariable("Hosting:Environment") ??
                  Environment.GetEnvironmentVariable("ASPNET_ENV")
                : initialConfig[HostDefaults.EnvironmentKey];

            environment ??= Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            environment = environment?.ToLower(); 
            
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = LoggerInit.InitializeSeriLog(config, environment, service);
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public static void SetUpEnvironment(IConfiguration configuration, string environment)
        {
            var configSection = configuration.GetSection("Connection");
            var settings = new Config();
            if (configSection.Exists()) configSection.Bind(settings);
            Config.Environment = environment;
        }

        public static void OnClosure()
        {
            Log.Information("Entered teardown. Deleting screenshots...");
            var path = Directory.GetFiles(TestContext.CurrentContext.WorkDirectory);
            foreach (var file in path)
            {
                if (file.Contains(".png"))
                {
                    try
                    {
                        File.Delete(file);
                        Log.Information($"Screenshot {file} was successfully deleted from container instance.");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Failed deleting {file}!");
                    }
                }
            }
            Log.CloseAndFlush();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Logger.Error(e.ExceptionObject as Exception, $"Current domain: unhandled exception occurred. IsTerminating={e.IsTerminating}");
            if (e.IsTerminating)
            {
                Log.CloseAndFlush();
            }
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Logger.Error(e.Exception, "Unobserved exception occurred.");
            e.SetObserved();
        }
    }
}