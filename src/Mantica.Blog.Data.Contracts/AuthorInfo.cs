namespace Mantica.Blog.Data.Contracts
{
    /// <summary>
    /// Holds author's bio
    /// </summary>
    public class AuthorInfo
    {
        /// <summary>
        /// Gets or sets author's name.
        /// </summary>
        /// <value>Name of the author.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets slub/ID of the author.
        /// </summary>
        /// <value>Author's slug/ID, e.g. john-doe.</value>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets bio.
        /// </summary>
        /// <value>Author's biography.</value>
        public string Bio { get; set; }

        /// <summary>
        /// Gets or sets language code.
        /// </summary>
        /// <value>Language used for the author information.</value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets URL of the author's avatar image.
        /// </summary>
        /// <value>Link to author's avatar picture.</value>
        public string AvatarUrl { get; set; }
    }
}