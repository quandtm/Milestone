using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using NGitHub;
using NGitHub.Models;
using RestSharp;

namespace Milestone.Model
{
    public class GitHubModel : INotifyPropertyChanged
    {
        private const string AuthFilename = "auth.dat";
        private const string RepoTOCFilename = "repotoc.dat";

        public bool IsAuthenticated { get; private set; }

        private GitHubClient _client;
        public User AuthenticatedUser { get; private set; }

        private string _username, _password;

        public GitHubModel()
        {
            _client = new GitHubClient();
        }

        public void Login(string username, string password, Action<bool> onComplete)
        {
            _username = null;
            _password = null;

            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            _client.Users.GetAuthenticatedUserAsync(
                new Action<User>(
                    u =>
                    {
                        if (u != null)
                        {
                            IsAuthenticated = true;
                            AuthenticatedUser = u;
                            _username = username;
                            _password = password;
                        }
                        else
                        {
                            IsAuthenticated = false;
                            AuthenticatedUser = null;
                        }

                        if (onComplete != null)
                            onComplete(IsAuthenticated);
                    }),
                new Action<GitHubException>(
                    ex =>
                    {
                        IsAuthenticated = false;
                        AuthenticatedUser = null;

                        if (onComplete != null)
                            onComplete(IsAuthenticated);
                    }));
        }

        public void Save()
        {

        }

        private void SaveAuth(IsolatedStorageFile isoStore)
        {

        }

        public void Load()
        {

        }

        private void LoadAuth(IsolatedStorageFile isoStore)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
