
using System.Reflection;

namespace Mantica.Blog.Common.Logging
{
    using System;
    using log4net;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implementation of <see cref="ILogger"/> backed by Log4Net.
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private readonly ILog logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogger"/> class.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        public Log4NetLogger(string loggerName)
        {
            logger = LogManager.GetLogger(Assembly.GetEntryAssembly(), loggerName);
        }

        /// <summary>
        /// Returns True if the specified <paramref name="logLevel"/> is enabled.
        /// </summary>
        /// <param name="logLevel"><see cref="Microsoft.Extensions.Logging.LogLevel"/> to check.</param>
        /// <returns>True if the specified <paramref name="logLevel"/> is enabled, False otherwise.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return logger.IsDebugEnabled;
                case LogLevel.Information:
                    return logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return logger.IsWarnEnabled;
                case LogLevel.Error:
                    return logger.IsErrorEnabled;
                case LogLevel.Critical:
                    return logger.IsFatalEnabled;
                default:
                    throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
            }
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">Scope <see cref="Type"/></typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<T>(LogLevel logLevel, EventId eventId, T state, Exception exception,
            Func<T, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message = null;

            if (null != formatter)
            {
                message = formatter(state, exception);
            }

            switch (logLevel)
            {
                case LogLevel.Debug:
                    logger.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    logger.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    logger.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    logger.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    logger.Fatal(message, exception);
                    break;
                default:
                    logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    logger.Info(message, exception);
                    break;
            }
        }
    }
}
