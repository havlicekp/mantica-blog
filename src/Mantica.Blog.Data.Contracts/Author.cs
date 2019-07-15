namespace Mantica.Blog.Data.Contracts
{
    /// <summary>
    /// Holds details about an author.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets author name.
        /// </summary>
        /// <value>Name of the author.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets information about the author.
        /// </summary>
        /// <value>The information about the author. It can contain multiple translations.</value>
        public AuthorInfo[] Infos { get; set; }
    }
}