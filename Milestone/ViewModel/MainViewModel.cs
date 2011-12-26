using GalaSoft.MvvmLight;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public GitHubModel Model { get; set; }

        public MainViewModel()
        {
            Model = ViewModelLocator.Model;
        }

        public void RefreshAll()
        {
            if (Model == null)
                return;

            RefreshRepositories();
        }

        public void RefreshRepositories()
        {
            if (Model == null)
                return;

            Model.RefreshRepos();
        }

        public void Logout()
        {
            if (Model == null)
                return;

            Model.Logout();
        }
    }
}