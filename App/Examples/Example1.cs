using Microsoft.Extensions.Logging;
using Sdk1Lib;

namespace App.Examples
{
    public class Example1 : AbstractExample
    {
        public Example1(SearchClient<SearchIndex> searchClient, ILogger logger) : base(searchClient, logger)
        {
        }

        public override string Description { get; } = "Using Microsoft.Azure.Search Sdk";

        //public async Task RunAsync()
        //{
        //    var searchModels = _searchModelBuilder.BuildSearchModels();
        //    await _searchClient.SaveAsync(searchModels);
        //    var count = await _searchClient.CountAsync();
        //    _logger.LogInformation("Azure search contains {count} items", count);
        //    await _searchClient.DeleteIndexAndDocumentsAsync();
        //}
    }
}
