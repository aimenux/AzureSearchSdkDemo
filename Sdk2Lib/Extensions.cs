using System;
using System.Diagnostics;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Core;
using AzureSearchIndex = Azure.Search.Documents.Indexes.Models.SearchIndex;

namespace Sdk2Lib
{
    public static class Extensions
    {
        public static SearchClient GetOrCreateSearchClient<T>(this SearchIndexClient searchIndexClient, Settings settings)
        {
            var indexName = settings.IndexName;

            var isIndexExists = IsIndexExists(searchIndexClient, indexName);
            if (!isIndexExists)
            {
                CreateIndex<T>(searchIndexClient, indexName);
            }

            return searchIndexClient.GetSearchClient(indexName);
        }

        private static void CreateIndex<T>(this SearchIndexClient searchIndexClient, string indexName)
        {
            var filedBuilder = new FieldBuilder();
            var indexDefinition = new AzureSearchIndex(indexName)
            {
                Fields = filedBuilder.Build(typeof(T))
            };

            searchIndexClient.CreateIndex(indexDefinition);
        }

        private static bool IsIndexExists(this SearchIndexClient searchIndexClient, string indexName)
        {
            try
            {
                var indexResponse = searchIndexClient.GetIndex(indexName);
                return indexResponse?.Value != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
