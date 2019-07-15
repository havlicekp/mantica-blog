namespace Mantica.Blog.Data.DocumentDB.Utils
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Newtonsoft.Json;

    /// <summary>
    /// Imports JSON documents in a folder into DocumentDb collection.
    /// File prefixes identifies type of the document.
    ///     - tbl.xxx => document
    ///     - sproc.xxx => stored procedure
    /// </summary>
    public class ScriptImporter
    {
        private readonly string path;
        private readonly string databaseId;
        private readonly string authKey;
        private readonly Uri collectionUri;
        private readonly Uri serviceEndpointUri;
        private readonly string collectionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptImporter"/> class.
        /// </summary>
        /// <param name="path">The path with the scripts to import into DocumentDB collection.</param>
        /// <param name="serviceEndpoint">The service endpoint. It can be obtained on http://portal.azure.com.</param>
        /// <param name="databaseId">The database ID.</param>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="authKey">The DocumentDB key. It can be obtained on http://portal.azure.com.</param>
        public ScriptImporter(string path, string serviceEndpoint, string databaseId, string collectionId, string authKey)
        {
            this.path = path;
            this.databaseId = databaseId;
            this.authKey = authKey;
            this.collectionId = collectionId;
            this.serviceEndpointUri = new Uri(serviceEndpoint);
            this.collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
        }

        /// <summary>
        /// Imports the scripts.
        /// </summary>
        public void Import()
        {
            using (var client = new DocumentClient(serviceEndpointUri, authKey))
            {
                this.ReCreateDocumentCollection(client);

                foreach (var file in Directory.GetFiles(path))
                {
                    var scriptName = Path.GetFileNameWithoutExtension(file);
                    if (string.IsNullOrEmpty(scriptName))
                    {
                        throw new ArgumentException("Invalid script name '" + scriptName + '"');
                    }

                    var scriptNameParts = scriptName.Split('.');
                    if (scriptNameParts.Length < 2)
                        throw new Exception($"Incorect name for the script '{file}'");

                    var scriptType = scriptNameParts.First();
                    var scriptId = string.Join(".", scriptNameParts.Skip(1));

                    switch (scriptType)
                    {
                        case "sproc":
                            CreateStoredProcedure(scriptId, file, client);
                            break;

                        case "tbl":
                            CreateDocument(file, client);
                            break;
                        default:
                            throw new Exception($"Invalid script type in '{file}'");
                    }
                }
            }
        }

        private void CreateDocument(string file, DocumentClient client)
        {
            dynamic obj = JsonConvert.DeserializeObject(File.ReadAllText(file));
            client.CreateDocumentAsync(collectionUri, obj).Wait();
        }

        private void CreateStoredProcedure(string scriptId, string file, DocumentClient client)
        {
            var sp = new StoredProcedure
            {
                Id = scriptId,
                Body = File.ReadAllText(file)
            };

            client.CreateStoredProcedureAsync(this.collectionUri, sp).Wait();
        }

        private void ReCreateDocumentCollection(DocumentClient client)
        {
            var dbUri = UriFactory.CreateDatabaseUri(this.databaseId);
            var coll = client.CreateDocumentCollectionQuery(dbUri)
                    .Where(c => c.Id == this.collectionId)
                    .AsEnumerable()
                .FirstOrDefault();

            // If the collection does exist, drop it
            if (coll != null)
            {
                client.DeleteDocumentCollectionAsync(this.collectionUri).Wait();
            }

            client.CreateDocumentCollectionAsync(
                    dbUri, new DocumentCollection {Id = this.collectionId}).Wait();
        }
    }
}
