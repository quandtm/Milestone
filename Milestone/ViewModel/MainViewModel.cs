using GalaSoft.MvvmLight;
using Milestone.Model;
using System;

namespace Milestone.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public GitHubModel Model { get; set; }

        public MainViewModel()
        {
            Model = ViewModelLocator.Model;
        }

        public void RefreshRepositories(Action<Context> contextComplete)
        {
            if (Model == null)
                return;

            Model.RefreshRepos(contextComplete);
        }

        public void Logout()
        {
            if (Model == null)
                return;

            Model.Logout();
        }
    }
}