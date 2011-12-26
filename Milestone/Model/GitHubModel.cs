using System;
using System.ComponentModel;
using NGitHub;
using NGitHub.Models;
using RestSharp;

namespace Milestone.Model
{
    public class GitHubModel : INotifyPropertyChanged
    {
        public bool IsAuthenticated { get; private set; }

        private GitHubClient _client;

        public GitHubModel()
        {
            _client = new GitHubClient();
        }

        public void Login(string username, string password, Action<bool> onComplete)
        {
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            _client.Users.GetAuthenticatedUserAsync(
                new Action<User>(
                    u =>
                    {
                        if (u != null)
                            IsAuthenticated = true;

                        if (onComplete != null)
                            onComplete(IsAuthenticated);
                    }),
                new Action<GitHubException>(
                    ex =>
                    {
                        IsAuthenticated = false;

                        if (onComplete != null)
                            onComplete(IsAuthenticated);
                    }));
            IsAuthenticated = true;

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
