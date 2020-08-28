using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace App.Examples
{
    public interface IExample
    {
        string Description { get; }
        Task CountAsync();
        Task DeleteAllAsync();
        Task UploadAsync(ICollection<ISearchModel> models);
    }
}
