using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Milestone.Model;

namespace Milestone.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public GitHubModel Model { get; set; }
        public Context SelectedContext { get; set; }

        public bool HasSelectedContext
        {
            get { return SelectedContext != null; }
        }

        private int _numBusy = 0;
        public bool IsBusy { get; set; }

        public MainViewModel(GitHubModel model)
        {
            Model = model;
        }

        public void Refresh(Action action)
        {
            if (SelectedContext != null && Model != null)
                Model.RefreshContextRepos(SelectedContext,
                                          () => // onStart
                                              {
                                                  ++_numBusy;
                                                  SetBusy();
                                              },
                                          () => // onComplete
                                              {
                                                  if (action != null)
                                                  action();
                                                  --_numBusy;
                                                  SetBusy();
                                                  RaisePropertyChanged("SelectedContext");
                                                  RaisePropertyChanged("SelectedContext.Repo");
                                              });
        }

        private void SetBusy()
        {
            IsBusy = _numBusy > 0;
        }

        public void Logout()
        {
            if (Model == null)
                return;

            Model.Logout();
        }

    }
}