using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SampleApp.DAL;

namespace SampleApp
{
    // Performs the search action.
    public class SearchController
    {
        private readonly MainWindowViewModel viewModel;
        private Task searchTask;
        private CancellationTokenSource cancellation;

        public SearchController(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;

            searchTask = InitialTask();
            cancellation = new CancellationTokenSource();
        }

        private Task InitialTask()
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            tcs.SetResult(0);
            return tcs.Task;
        }

        public void DoSearch()
        {
            SearchInfo searchInfo = new SearchInfo();
            MainWindowViewModel.SearchInfoProperties.SaveTo(searchInfo, viewModel);

            if (string.IsNullOrEmpty(searchInfo.FamilyName) && string.IsNullOrEmpty(searchInfo.GivenName))
            {
                viewModel.PeopleResults.Clear();
                return;
            }

            // Cancel any previous searches
            cancellation.Cancel();
            cancellation = new CancellationTokenSource();
            searchTask = InitialTask();

            MainWindowViewModel.SearchStatusProperty.SetValue(viewModel, "");

            // Do the search off the UI thread.
            searchTask = searchTask.ContinueWith<IEnumerable<Person>>(
                             previous => StartSearch(searchInfo),
                             cancellation.Token,
                             TaskContinuationOptions.OnlyOnRanToCompletion,
                             TaskScheduler.Default)

            // Update the ViewModel with the search results on the UI thread.
            .ContinueWith(
                             UpdateResults,
                             cancellation.Token,
                             TaskContinuationOptions.OnlyOnRanToCompletion,
                             TaskScheduler.FromCurrentSynchronizationContext());
        }

        private IEnumerable<Person> StartSearch(SearchInfo searchInfo)
        {
            var token = cancellation.Token;

            // Delay, to allow the user to continue typing.
            Thread.Sleep(2000);

            // A search triggered after this one might have occurred.
            token.ThrowIfCancellationRequested();

            MainWindowViewModel.SearchStatusProperty.SetValue(viewModel, "Searching");

            var query = DataContext.QueryPeople();

            if (!string.IsNullOrEmpty(searchInfo.GivenName))
                query = query.Where(p => p.Firstname.StartsWith(searchInfo.GivenName, StringComparison.CurrentCultureIgnoreCase));

            if (!string.IsNullOrEmpty(searchInfo.FamilyName))
                query = query.Where(p => p.Surname.StartsWith(searchInfo.FamilyName, StringComparison.CurrentCultureIgnoreCase));

            // Represents a delay in retrieving the information.
            Thread.Sleep(1000);

            token.ThrowIfCancellationRequested();

            return query.ToArray();
        }

        private void UpdateResults(Task<IEnumerable<Person>> t)
        {
            viewModel.PeopleResults.Clear();

            foreach (var result in t.Result)
                viewModel.PeopleResults.Add(new PersonViewModel(result));

            MainWindowViewModel.SearchStatusProperty.SetValue(viewModel, "");
        }
    }
}
