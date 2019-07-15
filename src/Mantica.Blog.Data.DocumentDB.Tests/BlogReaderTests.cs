namespace Mantica.Blog.Data.DocumentDB.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Mantica.Blog.Common;
    using Mantica.Blog.Data.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="BlogReader"/>
    /// </summary>
    [TestClass]
    public class BlogReaderTests : BlogTestsBase
    {
        /// <summary>
        /// Tests that an article with specified slug/id and languageCode is returned.
        /// </summary>
        [TestMethod]
        public async Task GetArticleReturnsSpecifiedArticle()
        {
            
            var author = ServiceProvider.GetService<IBlogAuthor>();
            var reader = ServiceProvider.GetService<IBlogReader>();

            var article = this.GenerateArticle();
            await author.CreateArticle(article);

            var memArticle = article.Versions.First();
            var dbArticle = await reader.GetArticle(memArticle.Slug, memArticle.LanguageCode);

            Assert.AreEqual(memArticle.Title, dbArticle.Title);
            Assert.AreEqual(memArticle.Summary, dbArticle.Summary);
            Assert.AreEqual(memArticle.Body, dbArticle.Body);
            Assert.AreEqual(memArticle.Slug, dbArticle.Slug);
            Assert.AreEqual(memArticle.LanguageCode, dbArticle.LanguageCode);
            Assert.AreEqual(memArticle.Metadata.Length, dbArticle.Metadata.Length);
            for (var i = 0; i < memArticle.Metadata.Length; i++)
            {
                Assert.IsTrue(MemberCompare.Equal(memArticle.Metadata[i], dbArticle.Metadata[i]));
            }
        }

        /// <summary>
        /// Tests that articles after specified id gets returned.
        /// </summary>
        [TestMethod]
        public async Task GetArticlesAfterSpecifiedIdTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();
            var reader = ServiceProvider.GetService<IBlogReader>();

            // Generate 5 articles > article.00001 .. article.00005
            for (int i = 0; i < 5; i++)
            {
                var article = this.GenerateArticle();
                await author.CreateArticle(article);
            }

            var articles = await reader.GetArticles("article.00003", 5, "en");

            Assert.AreEqual(2, articles.Length);
        }

        /// <summary>
        /// Tests that articles in a specified category metadata gets returned.
        /// </summary>
        [TestMethod]
        public async Task GetArticlesByCategoryTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();
            var reader = ServiceProvider.GetService<IBlogReader>();

            var article = this.GenerateArticle();
            await author.CreateArticle(article);

            // create second article with different category
            article.Versions[0].Metadata[0].Slug = "netherlands";
            await author.CreateArticle(article);

            var articles = await reader.GetArticles(null, 5, "en", MetadataType.Category, "netherlands");

            Assert.AreEqual(1, articles.Length);
            Assert.AreEqual("netherlands", articles[0].Metadata[0].Slug);
        }

        /// <summary>
        /// Tests that articles with specified author gets returned.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetArticlesByAuthorTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();
            var reader = ServiceProvider.GetService<IBlogReader>();

            var article = this.GenerateArticle();
            await author.CreateArticle(article);

            // create second article with different author for english version
            article.Versions.First(v => v.LanguageCode == "en").Author.Slug= "john-doe";
            await author.CreateArticle(article);

            var articles = await reader.GetArticles(null, 5, "en", "john-doe");

            Assert.AreEqual(1, articles.Length);
            Assert.AreEqual("john-doe", article.Versions.First(v => v.LanguageCode == "en").Author.Slug);
        }

        /// <summary>
        /// Tests that metadata gets returned.
        /// </summary>
        [TestMethod]
        public async Task GetMetadataTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();

            var metadata = await author.GetMetadata();
            metadata.Items = metadata.Items.Add(new MetadataItem
            {
                Versions = new []
                {
                    new MetadataVersion
                    {
                        LanguageCode = "en",
                        Name = "English Category",
                        Slug = "english-category",
                        Type = MetadataType.Category
                    },
                    new MetadataVersion
                    {
                        LanguageCode = "cs",
                        Name = "Czech Category",
                        Slug = "czech-category",
                        Type = MetadataType.Category
                    }
                }
            });

            metadata.Items = metadata.Items.Add(new MetadataItem
            {
                Versions = new[]
                {
                    new MetadataVersion
                    {
                        LanguageCode = "en",
                        Name = "English Tag",
                        Slug = "english-tag",
                        Type = MetadataType.Tag
                    },
                    new MetadataVersion
                    {
                        LanguageCode = "cs",
                        Name = "Czech Tag",
                        Slug = "czech-tag",
                        Type = MetadataType.Tag
                    }
                }
            });

            await author.UpdateMetadata(metadata);

            var reader = ServiceProvider.GetService<IBlogReader>();
            MetadataVersion[] versions = await reader.GetMetadata("cs");

            Assert.AreEqual(2, versions.Length);
            Assert.IsTrue(MemberCompare.Equal(metadata.Items[0].Versions[1], versions[0]));
            Assert.IsTrue(MemberCompare.Equal(metadata.Items[1].Versions[1], versions[1]));
        }
    }
}
