using System;

namespace Core
{
    public class Settings
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string IndexName { get; set; }

        public Uri BuildUri() => new Uri($"https://{Name}.search.windows.net/");
    }
}