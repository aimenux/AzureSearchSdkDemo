using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace App.Builders
{
    public class SearchModelBuilder : ISearchModelBuilder
    {
        private readonly int _size;

        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());

        public SearchModelBuilder(int size)
        {
            _size = size;
        }

        public ICollection<ISearchModel> BuildSearchModels()
        {
            if (_size <= 0)
            {
                throw new ArgumentException(nameof(_size));
            }

            return Enumerable.Range(0, _size)
                .Select(x => BuildRandomSearchModel())
                .ToList();
        }

        private static ISearchModel BuildRandomSearchModel()
        {
            return new SearchModel
            {
                PersonId = Guid.NewGuid().ToString(),
                FullName = BuildRandomString(20),
                Email = BuildRandomEmail(5, 5),
                BirthDate = BuildRandomDate()
            };
        }

        private static string BuildRandomString(int length)
        {
            const string chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray());
        }

        private static string BuildRandomEmail(int length1, int length2, string extension = "com")
        {
            return $"{BuildRandomString(length1)}@{BuildRandomString(length2)}.{extension}";
        }

        private static DateTime BuildRandomDate()
        {
            var years = -Random.Next(10, 50);
            return DateTime.Now.AddYears(years);
        }
    }
}