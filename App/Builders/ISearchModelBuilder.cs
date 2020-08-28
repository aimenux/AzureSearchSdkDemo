using System.Collections.Generic;
using System.Text;
using Core;

namespace App.Builders
{
    public interface ISearchModelBuilder
    {
        ICollection<ISearchModel> BuildSearchModels();
    }
}
