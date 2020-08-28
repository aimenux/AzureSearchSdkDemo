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
        private readonly ISearchClient _searchClient;
        private readonly ILogger _logger;

        protected AbstractExample(ISearchClient searchClient, ILogger logger)
        {
            _searchClient = searchClient;
            _logger = logger;
        }

        public abstract string Description { get; }

        public Task CountAsync()
        {
            var task = _searchClient.CountAsync();
            return TryCatchMonitorAsync(task);
        }

        public Task DeleteAllAsync()
        {
            var task = _searchClient.DeleteIndexAndDocumentsAsync();
            return TryCatchMonitorAsync(task);
        }

        public Task UploadAsync(ICollection<ISearchModel> models)
        {
            var task = _searchClient.SaveAsync(models);
            return TryCatchMonitorAsync(task);
        }

        private async Task TryCatchMonitorAsync(Task task, [CallerMemberName] string caller = null)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await task;
                stopWatch.Stop();
                _logger.LogInformation("{name} '{description}' took {duration}", caller, Description, stopWatch.Elapsed.ToString("g"));
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has occurred in {caller}: {ex}", caller, ex);
            }
        }
    }
}
