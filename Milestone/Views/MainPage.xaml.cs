using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
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

                _vm.RefreshAll();
            }

            base.OnNavigatedTo(e);
        }
    }
}