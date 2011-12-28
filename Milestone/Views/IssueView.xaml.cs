﻿using System.Windows.Navigation;
using Milestone.Extensions;
using Milestone.ViewModel;

namespace Milestone.Views
{
    public partial class IssueDetailsView
    {
        public IssueDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var contextIndex =  NavigationContext.TryGetKey("context");
            var repoName = NavigationContext.TryGetStringKey("repo");
            var issueId = NavigationContext.TryGetKey("id");
            

            var vm = (DataContext as IssueDetailsViewModel);
            if (vm != null && contextIndex.HasValue && issueId.HasValue)
            {
                vm.Id = issueId.Value;
                vm.ContextIndex = contextIndex.Value;
                vm.RepoName = repoName;
                //HACK: should we have an 'initialize' method rather than triggering on properties?
            }
            base.OnNavigatedTo(e);
        }
    }
}