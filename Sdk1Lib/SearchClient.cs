using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sdk1Lib
{
    public class SearchClient<TSearchIndex> : ISearchClient<TSearchIndex> where TSearchIndex : ISearchIndex
    {
        private readonly Settings _settings;
        private readonly ISearchIndexClient _searchIndexClient;
        private readonly SearchServiceClient _searchServiceClient;
        private readonly ILogger _logger;

        public SearchClient(IOptions<Settings> options, ILogger logger)
        {
            _logger = logger;
            _settings = options.Value;
            var credentials = new SearchCredentials(_settings.ApiKey);
            _searchServiceClient = new SearchServiceClient(_settings.Name, credentials);
            _searchIndexClient = _searchServiceClient.GetOrCreateSearchIndexClient<TSearchIndex>(_settings);
        }

        public Task<long> CountAsync()
        {
            return _searchIndexClient.Documents.CountAsync();
        }

        public Task DeleteIndexAndDocumentsAsync()
        {
            return _searchServiceClient.Indexes.DeleteAsync(_settings.IndexName);
        }

        public Task DeleteDocumentsAsync(string keyName, ICollection<string> keysValues)
        {
            var batch = IndexBatch.Delete(keyName, keysValues);
            return RunBatchAsync(batch);
        }

        public Task SaveAsync<TSearchModel>(ICollection<TSearchModel> models) where TSearchModel : ISearchModel
        {
            var actions = models.Select(IndexAction.Upload);
            var batch = IndexBatch.New(actions);
            return RunBatchAsync(batch);
        }

        public Task SaveAndOverwriteWhenExistsAsync<TSearchModel>(ICollection<TSearchModel> models) where TSearchModel : ISearchModel
        {
            var actions = models.Select(IndexAction.MergeOrUpload);
            var batch = IndexBatch.New(actions);
            return RunBatchAsync(batch);
        }

        public async Task<ICollection<TSearchModel>> GetAsync<TSearchModel>(string query, ISearchClientParameters parameters = null) where TSearchModel : ISearchModel
        {
            var searchParameters = parameters != null ? new SearchClientParameters(parameters) : null;
            var searchResults = await _searchIndexClient.Documents.SearchAsync<TSearchModel>(query, searchParameters);
            return searchResults.Results.Select(x => x.Document).ToList();
        }

        private async Task RunBatchAsync<T>(IndexBatch<T> batch, bool rethrow = true)
        {
            try
            {
                await _searchIndexClient.Documents.IndexAsync(batch);
            }
            catch (IndexBatchException ex)
            {
                var keys = ex
                    .IndexingResults
                    .Where(r => !r.Succeeded)
                    .Select(r => r.Key);

                var failedKeys = string.Join(", ", keys);

                _logger.LogError("Failed to index some documents: {keys} -> {ex}", failedKeys, ex);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
