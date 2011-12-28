using System;
using System.ComponentModel;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class NewCommentView : PhoneApplicationPage, INotifyPropertyChanged
    {
        private string _contextName, _repoName;
        private int _issueNumber;

        public bool IsBusy { get; set; }

        public NewCommentView()
        {
            InitializeComponent();
        }

        private void PostComment(object sender, EventArgs e)
        {
            var comment = txtComment.Text;
            if (string.IsNullOrWhiteSpace(comment))
                return;

            ViewModelLocator.Model.CreateComment(_contextName, _repoName, _issueNumber, comment,
                () =>
                {
                    IsBusy = true;
                },
                () =>
                {
                    ViewModelLocator.Dispatcher.BeginInvoke(() =>
                        {
                            IsBusy = false;
                            NavigationService.GoBack();
                        });
                });
        }

        private void Cancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string issueNum;
            if (!NavigationContext.QueryString.TryGetValue("context", out _contextName))
                NavigationService.GoBack();
            if (!NavigationContext.QueryString.TryGetValue("repo", out _repoName))
                NavigationService.GoBack();
            if (!NavigationContext.QueryString.TryGetValue("issue", out issueNum))
                NavigationService.GoBack();

            _issueNumber = int.Parse(issueNum);

            base.OnNavigatedTo(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}