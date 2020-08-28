using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public interface ISearchClient<TSearchIndex> : ISearchClient where TSearchIndex : ISearchIndex
    {
    }

    public interface ISearchClient
    {
        Task<long> CountAsync();
        Task DeleteIndexAndDocumentsAsync();
        Task DeleteDocumentsAsync(string keyName, ICollection<string> keysValues);
        Task SaveAsync<TSearchModel>(ICollection<TSearchModel> models) where TSearchModel : ISearchModel;
        Task SaveAndOverwriteWhenExistsAsync<TSearchModel>(ICollection<TSearchModel> models) where TSearchModel : ISearchModel;
        Task<ICollection<TSearchModel>> GetAsync<TSearchModel>(string query, ISearchClientParameters parameters = null) where TSearchModel : ISearchModel;
    }
}