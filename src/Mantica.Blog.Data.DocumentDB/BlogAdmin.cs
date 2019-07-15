namespace Mantica.Blog.Data.DocumentDB
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Contracts;

    /// <summary>
    /// Provides blog administrative metods.
    /// </summary>
    public class BlogAdmin : BlogAuthor, IBlogAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogAdmin"/> class.
        /// </summary>
        /// <param name="config">Configuration, it should contain DocumentDB connection settings.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/></param>
        public BlogAdmin(IConfigurationRoot config, ILoggerFactory loggerFactory) : base(config, loggerFactory)
        {
        }

        /// <summary>
        /// Returns blog authors.
        /// </summary>
        /// <returns>All the authors.</returns>
        public Task<Authors> GetAuthors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates blog authors
        /// </summary>
        /// <param name="authors">Document with the authors.</param>
        /// <returns><see cref="Task"/></returns>
        public Task UpdateAuthors(Authors authors)
        {
            throw new NotImplementedException();
        }
    }
}
