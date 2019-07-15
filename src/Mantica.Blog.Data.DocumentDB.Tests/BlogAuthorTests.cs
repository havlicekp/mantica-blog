namespace Mantica.Blog.Data.DocumentDB.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Mantica.Blog.Common;
    using Mantica.Blog.Data.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="BlogAdmin"/> class.
    /// </summary>
    [TestClass]
    public class BlogAuthorTests : BlogTestsBase
    {
        /// <summary>
        /// Tests that articles for a specified author gets returned.
        /// </summary>
        [TestMethod]
        public async Task GetArticlesForAuthorTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();

            for (int i = 0; i < 3; i++)
            {
                var article = GenerateArticle();
                await author.CreateArticle(article);
            }

            var articles = await author.GetArticles("john-smith");
            Assert.AreEqual(3, articles.Length);
        }

        /// <summary>
        /// Tests that an update to an article gets property persisted.
        /// </summary>
        [TestMethod]
        public async Task UpdateArticleTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();
            var article = GenerateArticle();
            await author.CreateArticle(article);

            article.Versions[0].Body = "Modified Article Body";
            article.State = ArticleState.Published;
            await author.UpdateArticle(article);

            var result = await author.GetArticles("john-smith");
            var dbArticle = result.First();

            Assert.AreEqual("article.00001", dbArticle.Id);
            Assert.AreEqual(article.Id, dbArticle.Versions[0].ArticleId);
            Assert.AreEqual(article.Id, dbArticle.Versions[1].ArticleId);
            Assert.AreEqual(article.State, dbArticle.State);
            Assert.AreEqual(article.Versions[0].Body, dbArticle.Versions[0].Body);
        }

        /// <summary>
        /// Tests that a new metadata record gets properly persited.
        /// </summary>
        [TestMethod]
        public async Task AddNewMetadataTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();

            var metadata = await author.GetMetadata();
            var objTag = new MetadataItem
            {
                Versions = new[] {
                    new MetadataVersion
                    {
                        Slug  = "tips", Type = MetadataType.Tag, LanguageCode = "en", Name = "Tips"
                    }
                }
            };

            metadata.Items = metadata.Items.Add(objTag);
            await author.UpdateMetadata(metadata);
            var dbMetadata = await author.GetMetadata();

            Assert.AreEqual(metadata.Items.Length, dbMetadata.Items.Length);

            for (int i = 0; i < metadata.Items.Length; i++)
            {
                for (int j = 0; j < metadata.Items[i].Versions.Length; j++)
                    Assert.IsTrue(MemberCompare.Equal(metadata.Items[i].Versions[j], dbMetadata.Items[i].Versions[j]));
            }
        }

        /// <summary>
        /// Tests that an article gets properly persisted.
        /// </summary>
        [TestMethod]
        public async Task CreateArticleTest()
        {
            var author = ServiceProvider.GetService<IBlogAuthor>();
            var article = this.GenerateArticle();
            await author.CreateArticle(article);

            var result = await author.GetArticles("john-smith");
            var dbArticle = result.First();

            Assert.AreEqual("article.00001", dbArticle.Id);
            Assert.AreEqual(article.State, dbArticle.State);
            Assert.AreEqual(article.Created, dbArticle.Created);
            Assert.AreEqual(article.Name, dbArticle.Name);
            for (int i = 0; i < article.Versions.Length; i++)
            {
                Assert.AreEqual(article.Versions[i].ArticleId, dbArticle.Versions[i].ArticleId);
                Assert.AreEqual(article.Versions[i].Body, dbArticle.Versions[i].Body);
                Assert.AreEqual(article.Versions[i].LanguageCode, dbArticle.Versions[i].LanguageCode);
                Assert.AreEqual(article.Versions[i].Slug, dbArticle.Versions[i].Slug);
                Assert.AreEqual(article.Versions[i].Summary, dbArticle.Versions[i].Summary);
                Assert.AreEqual(article.Versions[i].Title, dbArticle.Versions[i].Title);
            }
            Assert.IsTrue(MemberCompare.Equal(article.Versions[0].Metadata[0], article.Versions[0].Metadata[0]));
            Assert.IsTrue(MemberCompare.Equal(article.Versions[0].Metadata[1], article.Versions[0].Metadata[1]));
            Assert.IsTrue(MemberCompare.Equal(article.Versions[1].Metadata[0], article.Versions[1].Metadata[0]));
            Assert.IsTrue(MemberCompare.Equal(article.Versions[1].Metadata[1], article.Versions[1].Metadata[1]));
            Assert.IsTrue(MemberCompare.Equal(article.Versions[0].Author, article.Versions[0].Author));
        }
    }
}
