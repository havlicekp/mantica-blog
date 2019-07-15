namespace Mantica.Blog.Data.Contracts
{
    /// <summary>
    /// Holds details about a blog article.
    /// </summary>
    public class ArticleVersion
    {
        /// <summary>
        /// Gets or sets articleId.
        /// </summary>
        /// <value>The slug/ID of the article version. It should be the same as the parent <see cref="Article"/>.</value>
        public string ArticleId { get; set; }

        /// <summary>
        /// Gets or sets article body.
        /// </summary>
        /// <value>The article body translated into <see cref="LanguageCode"/></value>.
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets language code.
        /// </summary>
        /// <value>The language code used for thie <see cref="ArticleVersion"/>.</value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets article slug.
        /// </summary>
        /// <value>Article slug/ID, e.g. 'new-zealand'.</value>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets article summary.
        /// </summary>
        /// <value>The article summary.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets article title.
        /// </summary>
        /// <value>The article title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets author information.
        /// </summary>
        /// <value>Details about the article's author.</value>
        public AuthorInfo Author { get; set; }

        /// <summary>
        /// Gets or sets different article versions/translations.
        /// </summary>
        /// <value>The list of metadata items linked to the article. Typically categories and tags.</value>
        public MetadataVersion[] Metadata { get; set; }
    }
}