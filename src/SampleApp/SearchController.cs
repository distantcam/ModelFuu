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

            searchTask = searchTask.ContinueWith<IEnumerable<Person>>(previous =>
            {
                var token = cancellation.Token;

                // Delay, to allow the user to continue typing.
                Thread.Sleep(2000);

                MainWindowViewModel.SearchStatusProperty.SetValue(viewModel, "Searching");

                token.ThrowIfCancellationRequested();

                var query = DataContext.QueryPeople();

                if (!string.IsNullOrEmpty(searchInfo.GivenName))
                    query = query.Where(p => p.Firstname.StartsWith(searchInfo.GivenName, StringComparison.CurrentCultureIgnoreCase));

                if (!string.IsNullOrEmpty(searchInfo.FamilyName))
                    query = query.Where(p => p.Surname.StartsWith(searchInfo.FamilyName, StringComparison.CurrentCultureIgnoreCase));

                // Represents a delay in retrieving the information.
                Thread.Sleep(1000);

                return query.ToArray();
            }, cancellation.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default)

            .ContinueWith(t =>
            {
                viewModel.PeopleResults.Clear();

                foreach (var result in t.Result)
                    viewModel.PeopleResults.Add(new PersonViewModel(result));

                MainWindowViewModel.SearchStatusProperty.SetValue(viewModel, "");
            }, cancellation.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
