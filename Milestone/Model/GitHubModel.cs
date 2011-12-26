using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Threading;
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
        public ObservableCollection<Repository> Repos { get; private set; }

        private Dispatcher _dispatcher;

        private string _username, _password;

        public GitHubModel(Dispatcher dispatcher)
        {
            _client = new GitHubClient();
            Repos = new ObservableCollection<Repository>();
            _dispatcher = dispatcher;
        }

        public void Login(string username, string password, Action<bool> onComplete)
        {
            // TODO: Global progress bar
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

        public void RefreshRepos()
        {
            // TODO: Global progress bar
            if (!IsAuthenticated)
                return;

            _client.Users.GetRepositoriesAsync(AuthenticatedUser.Login,
                new Action<IEnumerable<Repository>>(
                    repos =>
                    {
                        //_dispatcher.BeginInvoke(new Action(() =>
                        //        {
                        //            Repos.Clear();
                        //            foreach (var repo in repos)
                        //                Repos.Add(repo);
                        //        }));
                    }),
                new Action<GitHubException>(
                    ex =>
                    {
                        MessageBox.Show(string.Format("Error: Could not get repos for {0}, the following exception occured: {1}", AuthenticatedUser.Login, ex.Message));
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
