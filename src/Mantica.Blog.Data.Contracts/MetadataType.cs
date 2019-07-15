namespace Mantica.Blog.Data.Contracts
{
    /// <summary>
    /// Indentifies metadata type
    /// </summary>
    public enum MetadataType
    {
        /// <summary>
        /// No type specified.
        /// </summary>
        None,

        /// <summary>
        /// Article Tag type.
        /// </summary>
        Tag,

        /// <summary>
        /// Article Category type.
        /// </summary>
        Category,

        /// <summary>
        /// Blog Language type.
        /// </summary>
        Language
    }
}