namespace Mantica.Blog.Data.Contracts
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Holds details about an article.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Gets or sets article ID.
        /// </summary>
        /// <value>Article ID, typically 'article.xxxx'.</value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets article name.
        /// </summary>
        /// <value>Article name. This value is used in the administration section.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the article state.
        /// </summary>
        /// <value>The article staet.</value>
        public ArticleState State { get; set; }

        /// <summary>
        /// Gets or sets different article versions/translations.
        /// </summary>
        /// <value>The article versions/translations.</value>
        public ArticleVersion[] Versions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> the article was created.
        /// </summary>
        /// <value><see cref="DateTime"/> when the article was created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> the article was published.
        /// </summary>
        /// <value><see cref="DateTime"/> when the article was published.</value>
        public DateTime Published { get; set; }
    }
}