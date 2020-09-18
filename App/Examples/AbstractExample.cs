using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core;
using Microsoft.Extensions.Logging;

namespace App.Examples
{
    public abstract class AbstractExample : IExample
    {
        protected readonly ISearchClient SearchClient;
        protected readonly ILogger Logger;

        protected AbstractExample(ISearchClient searchClient, ILogger logger)
        {
            SearchClient = searchClient;
            Logger = logger;
        }

        public abstract string Description { get; }

        public virtual Task CountAsync()
        {
            var task = SearchClient.CountAsync();
            return TryCatchMonitorAsync(task);
        }

        public Task SearchAsync()
        {
            const string searchText = "AT*";
            var task = SearchClient.GetAsync<SearchModel>(searchText);
            return TryCatchMonitorAsync(task);
        }

        public virtual Task DeleteAllAsync()
        {
            var task = SearchClient.DeleteIndexAndDocumentsAsync();
            return TryCatchMonitorAsync(task);
        }

        public virtual Task UploadAsync(ICollection<ISearchModel> models)
        {
            var task = SearchClient.SaveAsync(models);
            return TryCatchMonitorAsync(task);
        }

        protected async Task TryCatchMonitorAsync(Task task, [CallerMemberName] string caller = null)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await task;
                stopWatch.Stop();
                Logger.LogInformation("{name} '{description}' took {duration}", caller, Description, stopWatch.Elapsed.ToString("g"));
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                Logger.LogError("An error has occurred in {caller}: {ex}", caller, ex);
            }
        }
    }
}
