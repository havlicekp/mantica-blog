namespace Mantica.Blog.Data.Contracts
{
    using Newtonsoft.Json;

    /// <summary>
    /// Holds details about the blog.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets or sets document id. This should be set to 'metadata'.
        /// </summary>
        /// <value>The ID of the metadata document, typically 'metadata'.</value>
        [JsonProperty("id")]
        public string Id { get; set; } = "metadata";

        /// <summary>
        /// Gets or sets metadata.
        /// </summary>
        /// <value>The list of metadata items.</value>
        public MetadataItem[] Items { get; set; }
    }
}