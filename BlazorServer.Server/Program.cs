﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using static $safeprojectname$.Log;

namespace $safeprojectname$
{
    public class Program
    {
        // 設定 NLog 檔名
        static void SetNlogFileName()
        {
            _logFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            LogManager.Configuration.Variables["MY_DATE"] = _logFileName;
        }

        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            SetupTheLogger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            SetNlogFileName();
            try
            {
                Print("init main");
                Print("Log File Name:" + _logFileName);

                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                Print("Stopped program because of exception \n" + ex.ToString());
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .Build())
                .UseUrls("http://*:5000")
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()  // NLog: setup NLog for Dependency injection
                .Build();
    }
}
