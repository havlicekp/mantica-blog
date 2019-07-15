namespace Mantica.Blog.Data.Contracts
{
    /// <summary>
    /// Language specific version of metadata
    /// </summary>
    public class MetadataVersion
    {
        /// <summary>
        /// Gets or sets slug for the metadata
        /// </summary>
        /// <value>The slug/ID of the metadata, e.g. 'new-zealand'.</value>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets name of the metadata in its language.
        /// </summary>
        /// <value>Name of the metadata specified in the <see cref="LanguageCode"/></value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets metadata language.
        /// </summary>
        /// <value>The language used for this <see cref="MetadataVersion"/></value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets metadata type.
        /// </summary>
        /// <value>Metadata Type.</value>
        public MetadataType Type { get; set; }
    }
}