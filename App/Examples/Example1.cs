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
    }
}
