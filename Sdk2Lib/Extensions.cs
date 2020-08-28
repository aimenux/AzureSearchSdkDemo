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
            var isIndexExists = IsIndexExists(searchIndexClient, settings);
            if (!isIndexExists)
            {
                CreateIndex<T>(searchIndexClient, settings);
            }

            return searchIndexClient.GetSearchClient(settings.IndexName);
        }

        private static void CreateIndex<T>(this SearchIndexClient searchIndexClient, Settings settings)
        {
            var filedBuilder = new FieldBuilder();
            var indexDefinition = new AzureSearchIndex(settings.IndexName)
            {
                Fields = filedBuilder.Build(typeof(T))
            };

            searchIndexClient.CreateIndex(indexDefinition);
        }

        private static bool IsIndexExists(this SearchIndexClient searchIndexClient, Settings settings)
        {
            try
            {
                var indexResponse = searchIndexClient.GetIndex(settings.IndexName);
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
