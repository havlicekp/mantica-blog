namespace Mantica.Blog.Data.DocumentDB.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Common.Logging;
    using Mantica.Blog.Data.Contracts;
    using Mantica.Blog.Data.DocumentDB.Utils;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Base class for DocumentDB tests.
    /// </summary>
    public class BlogTestsBase
    {
        //
        // Without the random name in the collection name, Azure sometimes failed with 'Owner resource does not exist'
        // http://stackoverflow.com/questions/39040713/why-does-documentdb-fail-sporadically-when-run-in-a-test-senario
        //
        private readonly string collectionId = "mantica-test" + new Random().Next(1000, 9999);
        private IServiceProvider serviceProvider;
        private IConfigurationRoot config;

        /// <summary>
        /// Prepares environment for the test. It reads connection
        /// settings, sets-up IoC container and re-creates testing collection in DocumentDB.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            ReadConfig();
            SetupServiceProvider();
            SetupCollection();
        }

        /// <summary>
        /// Cleans up the DocumentDB after a test was run. It will remove the testi collection.
        /// </summary>
        [TestCleanup]
        public async Task CleanUp()
        {
            var serviceEndpoint = new Uri(this.config["manticaDocumentDbServiceEndpoint"]);
            var authKey = this.config["manticaDocumentDbAuthKey"];
            var dbId = this.config["manticaDocumentDbDatabaseId"];
            using (var client = new DocumentClient(serviceEndpoint, authKey))
            {
                var dbUri = UriFactory.CreateDatabaseUri(dbId);
                var colls = client.CreateDocumentCollectionQuery(dbUri)
                    .Where(c => c.Id.StartsWith("mantica-test"))
                    .AsEnumerable();

                foreach (var coll in colls)
                {
                    await client.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(dbId, coll.Id));
                }
            }
        }

        /// <summary>
        /// Gets IoC container.
        /// </summary>
        protected IServiceProvider ServiceProvider => serviceProvider;

        /// <summary>
        /// Creates a new instance of <see cref="Article"/> class filled with arbitrary data.
        /// </summary>
        /// <returns>The created <see cref="Article"/>.</returns>
        protected Article GenerateArticle()
        {
            var article = new Article
            {
                Id = null,
                Name = "Festival of the boats of light",
                Created = DateTime.Today,
                Published = DateTime.Now,
                State = ArticleState.Published
            };

            article.Versions = article.Versions.Add(new ArticleVersion
            {
                Body = "English body",
                LanguageCode = "en",
                Slug = "english-slug",
                Summary = "English summary",
                Title = "English title",
                Metadata = new[]
                {
                    new MetadataVersion
                    {
                        LanguageCode = "en",
                        Name = "New Zealand",
                        Type = MetadataType.Category,
                        Slug = "new-zealand"
                    },
                    new MetadataVersion
                    {
                        LanguageCode = "en",
                        Name = "Fishing",
                        Slug = "fishing",
                        Type = MetadataType.Tag
                    }
                },
                Author = new AuthorInfo
                {
                    AvatarUrl = "http://example.com/avatar.jpg",
                    Bio = "English Author bio",
                    LanguageCode = "en",
                    Name = "John Smith",
                    Slug = "john-smith"
                }
            });

            article.Versions = article.Versions.Add(new ArticleVersion
            {
                Body = "Czech body",
                LanguageCode = "cs",
                Slug = "czech-slug",
                Summary = "Czech summary",
                Title = "Czech title",
                Metadata = new[]
                {
                    new MetadataVersion
                    {
                        Slug = "novy-zeland",
                        LanguageCode = "cs",
                        Name = "Novy Zeland",
                        Type = MetadataType.Category,
                    },
                    new MetadataVersion
                    {
                        Slug = "rybareni",
                        LanguageCode = "cs",
                        Name = "Rybareni",
                        Type = MetadataType.Tag
                    }
                },
                Author = new AuthorInfo
                {
                    AvatarUrl = "http://example.com/avatar.jpg",
                    Bio = "Czech Author bio",
                    LanguageCode = "cs",
                    Name = "Jiri Smith",
                    Slug = "jiri-smith"
                }
            });

            return article;
        }

        private void SetupCollection()
        {
            var importer = new ScriptImporter(
                "./database/documentdb",
                config["manticaDocumentDbServiceEndpoint"],
                config["manticaDocumentDbDatabaseId"],
                collectionId,
                config["manticaDocumentDbAuthKey"]);
            importer.Import();
        }

        private void ReadConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Environment.CurrentDirectory);
            builder.AddUserSecrets<BlogTestsBase>();
            var builtConfig = builder.Build();
            builtConfig["manticaDocumentDbCollectionId"] = this.collectionId;
            this.config = builtConfig;
        }

        private void SetupServiceProvider()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddLog4Net("log4net.xml");

            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IBlogReader, BlogReader>();
            services.AddTransient<IBlogAuthor, BlogAuthor>();
            services.AddTransient<IBlogAdmin, BlogAdmin>();
            services.AddSingleton(this.config);
            services.AddSingleton(loggerFactory);

            this.serviceProvider = services.BuildServiceProvider();
        }
    }
}