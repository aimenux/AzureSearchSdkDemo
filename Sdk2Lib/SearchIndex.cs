using System;
using Azure.Search.Documents.Indexes;
using Core;

namespace Sdk2Lib
{
    public class SearchIndex : ISearchIndex
    {
        [SimpleField(IsKey = true)]
        public string PersonId { get; set; }

        [SearchableField(IsFilterable = true)]
        public string FullName { get; set; }

        [SearchableField(IsFilterable = true)]
        public string Email { get; set; }

        [SimpleField(IsSortable = true)]
        public DateTime BirthDate { get; set; }
    }
}
