namespace Mantica.Blog.Data.Contracts
{
    using Newtonsoft.Json;

    /// <summary>
    /// Holds details about blog authors
    /// </summary>
    public class Authors
    {
        /// <summary>
        /// Gets or sets id of the document. Should be set to 'authors'.
        /// </summary>
        /// <value>The ID of the document.</value>
        [JsonProperty("id")]
        public string Id { get; set; } = "authors";

        /// <summary>
        /// Gets or sets the list of the authors.
        /// </summary>
        /// <value>The list of the authors. </value>
        public Author[] Items { get; set; }
    }
}