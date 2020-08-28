using Microsoft.Extensions.Logging;
using Sdk2Lib;

namespace App.Examples
{
    public class Example2 : AbstractExample, IExample
    {
        public Example2(SearchClient<SearchIndex> searchClient, ILogger logger) : base(searchClient, logger)
        {
        }

        public override string Description { get; } = "Using Azure.Search.Documents Sdk";
    }
}
