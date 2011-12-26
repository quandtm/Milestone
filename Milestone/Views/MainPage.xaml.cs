using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Milestone.ViewModel;
using NGitHub.Models;

namespace Milestone.Views
{
    public partial class MainPage
    {
        private MainViewModel _vm;
        private bool _ignoreRepoSelection = false;
        private int _contextIndex;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm = (MainViewModel)DataContext;

            if (!_vm.Model.IsAuthenticated)
                NavigationService.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));
            else
            {
                if (NavigationContext.QueryString.ContainsKey("from") && NavigationContext.QueryString["from"].Equals("login"))
                    NavigationService.RemoveBackEntry();
            }

            base.OnNavigatedTo(e);
        }

        private void Logout(object sender, EventArgs e)
        {
            _vm.Logout();
            NavigationService.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));
        }

        private void SelectContext(object sender, SelectionChangedEventArgs e)
        {
            if (contextSelector.SelectedIndex == -1)
                return;

            _vm.SelectedContext = _vm.Model.Contexts[contextSelector.SelectedIndex];
            _contextIndex = contextSelector.SelectedIndex;
            _vm.Refresh();
        }

        private void SelectRepo(object sender, SelectionChangedEventArgs e)
        {
            if (_ignoreRepoSelection)
                return;

            var listbox = sender as ListBox;
            if (listbox == null || listbox.SelectedItem == null)
                return;

            var repo = listbox.SelectedItem as Repository;
            if (repo == null)
                return;

            // Reset the selection
            _ignoreRepoSelection = true;
            listbox.SelectedIndex = -1;
            _ignoreRepoSelection = false;

            NavigationService.Navigate(new Uri("/Views/IssueList.xaml?context=" + _contextIndex + "&repo=" + repo.Name.Replace(" ", "%20"), UriKind.Relative));
        }
    }
}