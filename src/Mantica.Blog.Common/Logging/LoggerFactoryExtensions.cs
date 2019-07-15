using System.Reflection;

namespace Mantica.Blog.Common.Logging
{
    using System;
    using System.IO;
    using log4net;
    using log4net.Config;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Extension methods for <see cref="ILoggerFactory"/>.
    /// </summary>
    public static class LoggerFactoryExtensions
    {
        /// <summary>
        /// Enables Log4Net to be added to <see cref="LoggerFactory"/>.
        /// </summary>
        /// <param name="loggerFactory"><see cref="ILoggerFactory"/> to add Log4Net to.</param>
        /// <param name="configFile">Path to the Log4Net config file.</param>
        public static void AddLog4Net(this ILoggerFactory loggerFactory, string configFile)
        {
            ConfigureLog4Net(configFile);
            loggerFactory.AddProvider(new Log4NetProvider());
        }

        private static void ConfigureLog4Net(string configFile = "log4net.config")
        {
            GlobalContext.Properties["appRoot"] = AppDomain.CurrentDomain.BaseDirectory;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(configFile));
        }
    }
}
