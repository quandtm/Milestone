using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

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
            if (true) // TODO: Replace with check to see if there is a logged in account
                NavigationService.Navigate(new Uri("/Views/Login.xaml", UriKind.Relative));

            base.OnNavigatedTo(e);
        }
    }
}