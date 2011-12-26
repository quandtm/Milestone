using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Milestone.Model;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MainViewModel _vm;

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
            _vm.Refresh();
        }
    }
}