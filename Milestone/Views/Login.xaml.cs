using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PerformLogin(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please specify a valid Username and Password.", "", MessageBoxButton.OK);
                return;
            }
            ViewModelLocator.Model.Login(txtUsername.Text, txtPassword.Password, new Action<bool>(b =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (b)
                        {
                            NavigationService.Navigate(new Uri("/Views/MainPage.xaml?from=login", UriKind.Relative));
                        }
                        else
                        {
                            MessageBox.Show("Login failed, please check your Username and Password and try again.", "", MessageBoxButton.OK);
                            btnLogin.IsEnabled = true;
                        }
                    }));
            }));
            btnLogin.IsEnabled = false;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            btnLogin.IsEnabled = true;
            base.OnNavigatedTo(e);
        }
    }
}