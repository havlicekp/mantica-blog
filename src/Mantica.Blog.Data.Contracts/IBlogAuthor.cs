namespace Mantica.Blog.Data.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides functions for blog authors.
    /// </summary>
    public interface IBlogAuthor
    {
        /// <summary>
        /// Persits a new articles in the database.
        /// </summary>
        /// <param name="article">Article to persist.</param>
        /// <returns>ID of the created article.</returns>
        Task<string> CreateArticle(Article article);

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="article">Article to update. ID of the article to update will be taken from <see cref="Article.Id"/></param>
        /// <returns><see cref="Task"/></returns>
        Task UpdateArticle(Article article);

        /// <summary>
        /// Returns list of articles written by a specified author.
        /// </summary>
        /// <param name="authorId">The ID of author whose articles to return.</param>
        /// <returns>The array of the articles.</returns>
        Task<Article[]> GetArticles(string authorId);

        /// <summary>
        /// Updates blog metadata (tags, categories, ..)
        /// </summary>
        /// <param name="metadata">Instance of the <see cref="Metadata"/> class with blog metadata to update.</param>
        /// <returns><see cref="Task"/></returns>
        Task UpdateMetadata(Metadata metadata);

        /// <summary>
        /// Gets blog metadata.
        /// </summary>
        /// <returns>The blog metadata records.B</returns>
        Task<Metadata> GetMetadata();
    }
}
