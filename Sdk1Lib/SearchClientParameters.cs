using Core;
using Microsoft.Azure.Search.Models;

namespace Sdk1Lib
{
    public class SearchClientParameters : SearchParameters, ISearchClientParameters
    {
        public SearchClientParameters()
        {
        }

        public SearchClientParameters(ISearchClientParameters parameters)
        {
            Top = parameters.Top;
            Filter = parameters.Filter;
        }
    }
}