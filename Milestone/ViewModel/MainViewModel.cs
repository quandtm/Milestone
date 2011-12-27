using GalaSoft.MvvmLight;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public GitHubModel Model { get; set; }
        public Context SelectedContext { get; set; }
        public bool HasSelectedContext { get { return SelectedContext != null; } }

        public MainViewModel(GitHubModel model)
        {
            Model = model;
        }

        public void Refresh()
        {
            if (SelectedContext != null && Model != null)
                Model.RefreshContextRepos(SelectedContext);
        }

        public void Logout()
        {
            if (Model == null)
                return;

            Model.Logout();
        }
    }
}