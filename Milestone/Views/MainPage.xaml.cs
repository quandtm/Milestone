using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!((MainViewModel)DataContext).Model.IsAuthenticated)
                NavigationService.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));

            base.OnNavigatedTo(e);
        }
    }
}