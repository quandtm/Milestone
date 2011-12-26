using System;
using System.ComponentModel;

namespace Milestone.Model
{
    public class GitHubModel : INotifyPropertyChanged
    {
        public bool IsAuthenticated { get; private set; }

        public void Login(string username, string password, Action<bool> onComplete)
        {
            IsAuthenticated = true;
            
            if (onComplete != null)
                onComplete(IsAuthenticated);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
