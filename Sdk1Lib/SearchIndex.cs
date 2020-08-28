using System;
using System.ComponentModel.DataAnnotations;
using Core;
using Microsoft.Azure.Search;

namespace Sdk1Lib
{
    public class SearchIndex : ISearchIndex
    {
        [Key]
        [IsFilterable]
        public string PersonId { get; set; }

        [IsFilterable, IsSearchable]
        public string FullName { get; set; }

        [IsFilterable]
        public string Email { get; set; }

        [IsSortable]
        public DateTime BirthDate { get; set; }
    }
}
