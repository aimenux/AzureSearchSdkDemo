using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.Extensions.Logging;
using Sdk2Lib;

namespace App.Examples
{
    public class Example2 : AbstractExample
    {
        public Example2(SearchClient<SearchIndex> searchClient, ILogger logger) : base(searchClient, logger)
        {
        }

        public override string Description { get; } = "Using Azure.Search.Documents Sdk";

        public override Task UploadAsync(ICollection<ISearchModel> models)
        {
            var searchModels = WorkaroundSdk2ConversionToConcreteType(models).ToList();
            var task = SearchClient.SaveAsync(searchModels);
            return TryCatchMonitorAsync(task);
        }

        private static IEnumerable<SearchModel> WorkaroundSdk2ConversionToConcreteType(IEnumerable<ISearchModel> models)
        {
            foreach (var model in models)
            {
                if (model is SearchModel searchModel)
                {
                    yield return searchModel;
                }
            }
        }
    }
}
