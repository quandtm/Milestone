using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Milestone.Views
{
    public partial class AboutView : PhoneApplicationPage
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void OpenFeedback(object sender, RoutedEventArgs e)
        {
            var wbt = new WebBrowserTask();
            wbt.Uri = new Uri(@"https://github.com/quandtm/Milestone/issues", UriKind.Absolute);
            wbt.Show();
        }
    }
}