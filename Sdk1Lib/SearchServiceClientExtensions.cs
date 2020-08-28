using System;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Sdk1Lib
{
    public static class SearchServiceClientExtensions
    {
        public static ISearchIndexClient GetOrCreateSearchIndexClient<T>(this ISearchServiceClient searchServiceClient, string indexName)
        {
            if (!searchServiceClient.Indexes.Exists(indexName))
            {
                searchServiceClient.CreateIndex<T>(indexName);
            }

            return searchServiceClient.Indexes.GetClient(indexName);
        }

        private static void CreateIndex<T>(this ISearchServiceClient searchServiceClient, string indexName)
        {
            var indexDefinition = new Index
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            var index = searchServiceClient.Indexes.Create(indexDefinition);
            if (index == null)
            {
                throw new Exception($"Failed to create index {indexName}");
            }
        }
    }
}
