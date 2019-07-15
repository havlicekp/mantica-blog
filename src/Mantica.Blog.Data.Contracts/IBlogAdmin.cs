namespace Mantica.Blog.Data.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides blog administrative metods.
    /// </summary>
    public interface IBlogAdmin
    {
        /// <summary>
        /// Returns blog authors.
        /// </summary>
        /// <returns>All the authors.</returns>
        Task<Authors> GetAuthors();

        /// <summary>
        /// Updates blog authors
        /// </summary>
        /// <param name="authors">Document with the authors.</param>
        /// <returns><see cref="Task"/></returns>
        Task UpdateAuthors(Authors authors);
    }
}
