namespace Mantica.Blog.Data.Contracts
{
    /// <summary>
    /// Holds metadata details and translations.
    /// </summary>
    public class MetadataItem
    {
        /// <summary>
        /// Gets or sets metadata name.
        /// </summary>
        /// <value>
        /// Name of the metadata item. This value is used in the administration.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets metadata versions.
        /// </summary>
        /// <value>The information about the metadata item. It can contain multiple translations.</value>
        public MetadataVersion[] Versions { get; set; }
    }
}