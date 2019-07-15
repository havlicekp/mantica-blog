namespace Mantica.Blog.Data.Contracts
{
    /// <summary>
    /// Holds details about a blog language.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Gets or sets language code.
        /// </summary>
        /// <value>
        /// Language code in two-letter code as specified in ISO 639-1.
        /// https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
        /// </value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets language name.
        /// </summary>
        /// <value>Lanugage name translated to the language itself, e.g. 'Česky' for Czech.</value>
        public string Name { get; set; }
    }
}