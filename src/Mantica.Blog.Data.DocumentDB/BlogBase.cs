namespace Mantica.Blog.Data.DocumentDB
{
    using System;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Linq;


    /// <summary>
    /// Base clas for the blog implementation.
    /// </summary>
    public class BlogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogBase"/> class.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public BlogBase(IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            this.ServiceEndpoint = config["manticaDocumentDbServiceEndpoint"];
            this.AuthKey = config["manticaDocumentDbAuthKey"];
            this.DatabaseId = config["manticaDocumentDbDatabaseId"];
            this.CollectionId = config["manticaDocumentDbCollectionId"];
            this.CollectionUri = UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId);
            this.CounterMapper = new CounterMapper();
            this.Log = loggerFactory.CreateLogger(this.GetType().Name);

            var connPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };
            this.Client = new DocumentClient(new Uri(this.ServiceEndpoint), this.AuthKey, connPolicy);
        }

        /// <summary>
        /// Gets logger.
        /// </summary>
        protected ILogger Log { get; }

        /// <summary>
        /// Gets DocumentDb service endpoint. Can be obtained at portal.azure.com.
        /// </summary>
        protected string ServiceEndpoint { get; }

        /// <summary>
        /// Gets DocumentDb key. Can be obtained at portal.azure.com.
        /// </summary>
        protected string AuthKey { get; }

        /// <summary>
        /// Gets DocumentDb database id.
        /// </summary>
        protected string DatabaseId { get; }

        /// <summary>
        /// Gets DocumentDb collection id.
        /// </summary>
        protected string CollectionId { get; }

        /// <summary>
        /// Gets DocumentDb collection URI.
        /// </summary>
        protected Uri CollectionUri { get; }

        /// <summary>
        /// Ges DocumentDb client.
        /// </summary>
        protected DocumentClient Client { get; }

        /// <summary>
        /// Gets counter mapper.
        /// </summary>
        protected CounterMapper CounterMapper { get; }

        /// <summary>
        /// Executes the specified <see cref="IDocumentQuery{T}"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>The array of records returned by the query.</returns>
        protected async Task<T[]> ExecutedQueryAsync<T>(IDocumentQuery<T> query)
        {
            var result = new List<T>();
            double requestCharge = 0;
            while (query.HasMoreResults)
            {
                FeedResponse<T> queryResponse = await query.ExecuteNextAsync<T>();
                result.AddRange(queryResponse.ToArray());
                requestCharge += queryResponse.RequestCharge;
            }

            Log.LogInformation($"Query '{this.UnescapeQuotes(query.ToString())}', cost {requestCharge} RUs");
            return result.ToArray();
        }

        private string UnescapeQuotes(string value)
        {
            return value.Replace("\\\"", "\"");
        }
    }
}