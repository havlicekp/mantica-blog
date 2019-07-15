namespace Mantica.Blog.Data.DocumentDB
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Contracts;

    /// <summary>
    /// Provides methods for reading the blog.
    /// </summary>
    public class BlogReader : BlogBase, IBlogReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogReader"/> class.
        /// </summary>
        /// <param name="config">Configuration, it should contain DocumentDB connection settings.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/></param>
        public BlogReader(IConfigurationRoot config, ILoggerFactory loggerFactory) : base(config, loggerFactory)
        {
        }

        /// <summary>
        /// Returns an article with the specified slug (ID) and languageCode.
        /// Used for direct url like http://example.com/en/article-slug/.
        /// </summary>
        /// <param name="slug">Slug/ID of the article to return.</param>
        /// <param name="languageCode">The language code in which is the article written.</param>
        /// <returns>The article with the specified slug.</returns>
        public async Task<ArticleVersion> GetArticle(string slug, string languageCode)
        {
            var query = this.Client.CreateDocumentQuery<Article>(this.CollectionUri)
                .SelectMany(article => article.Versions
                    .Where(v => v.LanguageCode == languageCode && v.Slug == slug)
                    .Select(v => v))
                .AsDocumentQuery();

            var result = await this.ExecutedQueryAsync(query);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Returns specified number of articles written in the <paramref name="languageCode"/>.
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <returns>The array of articles.</returns>
        public async Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode)
        {
            var query = this.Client.CreateDocumentQuery<Article>(this.CollectionUri)
                .Where(d =>
                        (string.IsNullOrEmpty(startArticleId) || d.Id.CompareTo(startArticleId) > 0) &&
                        d.State == ArticleState.Published
                )
                .OrderByDescending(d => d.Published)
                .Take(count)
                .SelectMany(article => article.Versions
                    .Where(v => v.LanguageCode == languageCode))
                .Select(v => v)
                .AsDocumentQuery();

            return await this.ExecutedQueryAsync(query);
        }

        /// <summary>
        /// Returns the specified number of articles having the metadata set (category, tag, ..).
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <param name="type">The <see cref="MetadataType"/> of metadata to look for (category, tag, ..).</param>
        /// <param name="metadataSlug">Slug/ID of the metadata.</param>
        /// <returns>The array of articles having the metadata set.</returns>
        public async Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode, MetadataType type, string metadataSlug)
        {
            var query = this.Client.CreateDocumentQuery<Article>(this.CollectionUri)
                .Where(d =>
                        (string.IsNullOrEmpty(startArticleId) || d.Id.CompareTo(startArticleId) > 0) &&
                        d.State == ArticleState.Published
                )
                .OrderByDescending(d => d.Published)
                .Take(count)
                .SelectMany(article => article.Versions
                    .SelectMany(version => version.Metadata
                        .Where(metadata =>
                            metadata.Slug == metadataSlug &&
                            version.LanguageCode == languageCode &&
                            article.State == ArticleState.Published)
                        .Select(result => version)))
                .AsDocumentQuery();

            return await this.ExecutedQueryAsync(query);
        }

        /// <summary>
        /// Returns specified number of articles written by the autor.
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <param name="authorSlug">Articles written by the specified author will be returned.</param>
        /// <returns>The array of articles written by the author in the specified language.</returns>
        public async Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode, string authorSlug)
        {
            var query = this.Client.CreateDocumentQuery<Article>(this.CollectionUri)
                .Where(d =>
                        (string.IsNullOrEmpty(startArticleId) || d.Id.CompareTo(startArticleId) > 0) &&
                        d.State == ArticleState.Published
                )
                .OrderByDescending(d => d.Published)
                .Take(count)
                .SelectMany(article => article.Versions
                    .Where(version =>
                        version.Author.Slug == authorSlug &&
                        version.LanguageCode == languageCode &&
                        article.State == ArticleState.Published)
                    .Select(version => version))
                .AsDocumentQuery();

            return await this.ExecutedQueryAsync(query);
        }

        /// <summary>
        /// Returns blog metadata (tags, categories, ..) for specified language.
        /// </summary>
        /// <returns>The array of blog metadata records.</returns>
        public async Task<MetadataVersion[]> GetMetadata(string languageCode)
        {
            var query = this.Client.CreateDocumentQuery<Metadata>(this.CollectionUri)
                .Where(m => m.Id == "metadata")
                .SelectMany(m => m.Items
                    .SelectMany(item => item.Versions)
                    .Where(version => version.LanguageCode == languageCode))
                .Select(version => version)
                .AsDocumentQuery();

            return await this.ExecutedQueryAsync(query);
        }
    }
}
