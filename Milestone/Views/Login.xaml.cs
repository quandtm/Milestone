using System;
using System.Windows;
using Microsoft.Phone.Shell;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class Login
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PerformLogin(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please specify a valid Username and Password.", "", MessageBoxButton.OK);
                return;
            }

            ViewModelLocator.Model.Login(txtUsername.Text, txtPassword.Password, (b =>
            {
                Dispatcher.BeginInvoke((() =>
                    {
                        if (b)
                        {
                            NavigationService.Navigate(new Uri("/Views/MainPage.xaml?from=login", UriKind.Relative));
                        }
                        else
                        {
                            MessageBox.Show("Login failed, please check your Username and Password and try again.", "", MessageBoxButton.OK);
                            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true; //Not accessible through name. 
                            shellProgress.IsVisible = false;
                        }
                    }));
            }));
            shellProgress.IsVisible = true;
            ((ApplicationBarIconButton) ApplicationBar.Buttons[0]).IsEnabled = false; //Not accessible through name. 
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true; //Not accessible through name. 
            base.OnNavigatedTo(e);
        }
    }
}