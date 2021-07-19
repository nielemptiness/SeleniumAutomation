using System;
using Core.Base.Startup;
using NUnit.Framework;
using Serilog;

namespace Tests
{
    [SetUpFixture]
    public class Program
    {
        public static void Main(string[] args)
        {
            Args = args;
        }

        private static string[] Args;
        
        private const string Service = "AutomationExample";

        [OneTimeSetUp]
        public static void StartApplication()
        {   
            Start.CreateConfiguration(Args, Service, out var env, out var config);
            
            try
            {
                Log.Information("Starting tests...");
                Start.SetUpEnvironment(config, env);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Exception occurred while starting tests.");
                throw;
            }
        }
        
        [OneTimeTearDown]
        public static void TearDown()
        {
            Start.OnClosure();
        }
    }
}