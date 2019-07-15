namespace Mantica.Blog.Service.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Mantica.Blog.Common.Exceptions;
    using Mantica.Blog.Data.Contracts;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Read controller.
    /// </summary>
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    public class ReadController : Controller
    {
        private readonly IBlogReader blogReader;
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadController"/> class.
        /// </summary>
        /// <param name="blogReader">The blog reader.</param>
        /// <param name="logFactory">The log factory</param>
        public ReadController(IBlogReader blogReader, ILoggerFactory logFactory)
        {
            this.blogReader = blogReader;
            this.log = logFactory.CreateLogger(typeof(ReadController).Name);
        }

        /// <summary>
        /// GET api/read/en/article-slug
        /// </summary>
        /// <param name="slug">Slug/ID of the article to return.</param>
        /// <param name="languageCode">The language code in which is the article written.</param>
        /// <returns>The article with the specified slug.</returns>
        [HttpGet("{languageCode}/{slug}")]
        public async Task<ArticleVersion> GetArticle(string languageCode, string slug)
        {
            var article = await blogReader.GetArticle(slug, languageCode);
            return article;
        }

        /// <summary>
        /// Returns specified number of articles written in the <paramref name="languageCode"/>.
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <returns>The array of articles.</returns>
        [HttpGet("{languageCode}/{startArticleId}/{count}")]
        public async Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode)
        {
            var articles = await blogReader.GetArticles(startArticleId, count, languageCode);
            return articles;
        }

        /// <summary>
        /// Returns the specified number of articles having the metadata set (category, tag, ..).
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <param name="type">The <see cref="MetadataType"/> of metadata to look for (category, tag, ..).</param>
        /// <param name="metadataSlug">Slug/ID of the metadata.</param>
        /// <returns>The array of articles having the metadata set.</returns>
        [HttpGet("metadata/{languageCode}/{startArticleId}/{count}/{type}/{metadataSlug}")]
        public async Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode, string type,
            string metadataSlug)
        {
            MetadataType parsedType;
            if (Enum.TryParse<MetadataType>(type, true, out parsedType))
            {
                var articles =
                    await blogReader.GetArticles(startArticleId, count, languageCode, parsedType, metadataSlug);
                return articles;
            }
            else
            {
                throw new BlogException("Unable to parse the metadata type");
            }
        }

        /// <summary>
        /// Returns specified number of articles written by the autor.
        /// </summary>
        /// <param name="startArticleId">If set only articles with higher ID will be returned. Can be 0 or null.</param>
        /// <param name="count">Number of articles to return.</param>
        /// <param name="languageCode">Articles with the specified language will be returned.</param>
        /// <param name="authorSlug">Articles written by the specified author will be returned.</param>
        /// <returns>The array of articles written by the author in the specified language.</returns>
        [HttpGet("author/{languageCode}/{startArticleId}/{count}/{authorSlug}")]
        public async Task<ArticleVersion[]> GetArticles(string startArticleId, int count, string languageCode,
            string authorSlug)
        {
            var articles = blogReader.GetArticles(startArticleId, count, languageCode, authorSlug);
            return await articles;
        }

        /*
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
