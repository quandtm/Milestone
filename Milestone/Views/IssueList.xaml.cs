using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Milestone.Views
{
    public partial class IssueList : PhoneApplicationPage
    {
        public IssueList()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var contextIndex = int.Parse(NavigationContext.QueryString["context"]);
            var repoName = NavigationContext.QueryString["repo"];

            base.OnNavigatedTo(e);
        }
    }
}