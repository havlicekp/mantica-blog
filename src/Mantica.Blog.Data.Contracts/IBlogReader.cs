namespace Mantica.Blog.Data.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for reading the blog.
    /// </summary>
    public interface IBlogReader
    {
        /// <summary>
        /// Retunrs an article with the specified slug (ID) and languageCode.
        /// Used for direct url like http://example.com/en/article-slug/.
        /// </summary>
        /// <param name="slug">Slug/ID of the article to return.</param>
        /// <param name="languageCode">The language code in which is the article written.</param>
        /// <returns>The article with the specified slug.</returns>
        Task<ArticleVersion> GetArticle(string slug, string languageCode);

        /// <summary>
        /// Returns specified number of articles written in the <paramref name="languageCode"/>.
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <returns>The array of articles.</returns>
        Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode);

        /// <summary>
        /// Returns the specified number of articles having the metadata set (category, tag, ..).
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <param name="type">The <see cref="MetadataType"/> of metadata to look for (category, tag, ..).</param>
        /// <param name="metadataSlug">Slug/ID of the metadata.</param>
        /// <returns>The array of articles having the metadata set.</returns>
        Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode, MetadataType type, string metadataSlug);

        /// <summary>
        /// Returns specified number of articles written by the autor.
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <param name="authorId">Articles written by the specified author will be returned.</param>
        /// <returns>The array of articles written by the author in the specified language.</returns>
        Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode, string authorId);

        /// <summary>
        /// Returns blog metadata (tags, categories, ..) for specified language.
        /// </summary>
        /// <param name="languageCode">The language for which the metadata should be returned.</param>
        /// <returns>The array of blog metadata records.</returns>
        Task<MetadataVersion[]> GetMetadata(string languageCode);
    }
}
