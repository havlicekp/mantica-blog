namespace Mantica.Blog.Common.Logging
{
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implementation of <see cref="ILoggerProvider"/> for Log4Net.
    /// </summary>
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Log4NetLogger> loggers
            = new ConcurrentDictionary<string, Log4NetLogger>();

        /// <summary>
        /// Creates <see cref="ILogger"/> backed by Log4Net.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string name)
        {
            return loggers.GetOrAdd(name, new Log4NetLogger(name));
        }

        /// <summary>
        /// Removes all the registered loggers.
        /// </summary>
        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
