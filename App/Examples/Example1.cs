using System.Threading.Tasks;
using App.Builders;
using Core;
using Microsoft.Extensions.Logging;
using Sdk1Lib;

namespace App.Examples
{
    public class Example1 : IExample
    {
        private readonly ISearchModelBuilder _searchModelBuilder;
        private readonly ISearchClient<SearchIndex> _searchClient;
        private readonly ILogger _logger;

        public Example1(ISearchModelBuilder searchModelBuilder, ISearchClient<SearchIndex> searchClient, ILogger logger)
        {
            _logger = logger;
            _searchClient = searchClient;
            _searchModelBuilder = searchModelBuilder;
        }

        public string Description { get; } = "Using Microsoft.Azure.Search Sdk";

        public async Task RunAsync()
        {
            await _searchClient.DeleteIndexAndDocumentsAsync();
            var searchModels = _searchModelBuilder.BuildSearchModels();
            await _searchClient.SaveAsync(searchModels);
            var countItems = _searchClient.CountAsync();
            _logger.LogInformation("Azure search contains {count} items", countItems);
        }
    }
}
