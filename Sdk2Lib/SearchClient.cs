using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sdk2Lib
{
    public class SearchClient<TSearchIndex> : ISearchClient<TSearchIndex> where TSearchIndex : ISearchIndex
    {
        private readonly Settings _settings;
        private readonly SearchClient _searchClient;
        private readonly SearchIndexClient _searchIndexClient;
        private readonly ILogger _logger;

        public SearchClient(IOptions<Settings> options, ILogger logger)
        {
            _logger = logger;
            _settings = options.Value;
            var credentials = new AzureKeyCredential(_settings.ApiKey);
            _searchIndexClient = new SearchIndexClient(_settings.BuildUri(), credentials);
            _searchClient = _searchIndexClient.GetOrCreateSearchClient<TSearchIndex>(_settings);
        }

        public async Task<long> CountAsync()
        {
            var response = await _searchClient.GetDocumentCountAsync();
            return response.Value;
        }

        public Task DeleteIndexAndDocumentsAsync()
        {
            return _searchIndexClient.DeleteIndexAsync(_settings.IndexName);
        }

        public Task DeleteDocumentsAsync(string keyName, ICollection<string> keysValues)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync<TSearchModel>(ICollection<TSearchModel> models) where TSearchModel : ISearchModel
        {
            return _searchClient.UploadDocumentsAsync(models);
        }

        public Task SaveAndOverwriteWhenExistsAsync<TSearchModel>(ICollection<TSearchModel> models) where TSearchModel : ISearchModel
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TSearchModel>> GetAsync<TSearchModel>(string query, ISearchClientParameters parameters = null) where TSearchModel : ISearchModel
        {
            throw new NotImplementedException();
        }
    }
}
