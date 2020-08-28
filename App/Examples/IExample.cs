using System.Threading.Tasks;

namespace App.Examples
{
    public interface IExample
    {
        public string Description { get; }
        public Task RunAsync();
    }
}
