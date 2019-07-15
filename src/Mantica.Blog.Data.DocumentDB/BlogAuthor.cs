namespace Mantica.Blog.Data.DocumentDB
{
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides functions for blog authors.
    /// </summary>
    public class BlogAuthor : BlogReader, IBlogAuthor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BlogAuthor"/> class.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="loggerFactory"></param>
        public BlogAuthor(IConfigurationRoot config, ILoggerFactory loggerFactory) : base(config, loggerFactory)
        {
        }

        /// <summary>
        /// Persits a new articles in the database.
        /// </summary>
        /// <param name="article"></param>
        /// <returns>ID of the created article.</returns>
        public async Task<string> CreateArticle(Article article)
        {
            // Sproc will assign an unique ID to the article. It uses article.counter document
            // to track the last ID used
            var spUri = UriFactory.CreateStoredProcedureUri(this.DatabaseId, this.CollectionId,
                "insertWithIncrementedId");
            var counter = this.CounterMapper.FromType(article);
            var result = await this.Client.ExecuteStoredProcedureAsync<string>(spUri, article, counter);

            Log.LogInformation($"Article '{result.Response}' created, cost {result.RequestCharge} RUs");

            return article.Id = result.Response;
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="article">Article to update. ID of the article to update will be taken from <see cref="Article.Id"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateArticle(Article article)
        {
            // propagate articleId to all article translations/versions,
            // might be better to do in a sproc
            article.Versions.ToList().ForEach(v => v.ArticleId = article.Id);

            var result = await this.Client.UpsertDocumentAsync(this.CollectionUri, article);

            Log.LogInformation($"Article '{article.Id}' updated, cost {result.RequestCharge} RUs");
        }

        /// <summary>
        /// Returns list of articles written by a specified author.
        /// </summary>
        /// <returns>The array of all articles in the blog.</returns>
        public async Task<Article[]> GetArticles(string authorId)
        {
            var query = this.Client.CreateDocumentQuery<Article>(this.CollectionUri)
                .Where(articles => articles.Id.StartsWith("article."))
                .SelectMany(article => article.Versions
                .Where(version => version.Author.Slug == authorId)
                .Select(result => article))
                .AsDocumentQuery();

            return await this.ExecutedQueryAsync(query);
        }

        /// <summary>
        /// Updates blog metadata (tags, categories, ..)
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateMetadata(Metadata metadata)
        {
            var result = await this.Client.UpsertDocumentAsync(this.CollectionUri, metadata);

            Log.LogInformation($"Updated metadata, cost {result.RequestCharge} RUs");
        }

        /// <summary>
        /// Gets blog metadata.
        /// </summary>
        /// <returns>The blog metadata records.B</returns>
        public async Task<Metadata> GetMetadata()
        {
            var query = this.Client.CreateDocumentQuery<Metadata>(this.CollectionUri)
                .Where(m => m.Id == "metadata")
                .AsDocumentQuery();

            var result = await this.ExecutedQueryAsync(query);
            return result.FirstOrDefault();
        }
    }
}
