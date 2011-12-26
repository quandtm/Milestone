using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Threading;
using NGitHub;
using NGitHub.Models;
using NGitHub.Web;
using RestSharp;

namespace Milestone.Model
{
    public class GitHubModel : INotifyPropertyChanged
    {
        private const string AuthFilename = "auth.dat";
        private const int AuthFileVersion = 1;
        private const string RepoFilename = "repotoc.dat";
        private const int RepoFileVersion = 1;

        public bool IsAuthenticated { get; private set; }

        private GitHubClient _client;
        public User AuthenticatedUser { get; private set; }
        public ObservableCollection<Context> Contexts { get; private set; }

        public Dispatcher Dispatcher { get; set; }

        private string _username, _password;

        public GitHubModel()
        {
            _client = new GitHubClient();

            Contexts = new ObservableCollection<Context>();
        }

        public void Login(string username, string password, Action<bool> onComplete)
        {
            // TODO: Global progress bar
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            _client.Users.GetAuthenticatedUserAsync(
                new Action<User>(
                    u =>
                    {
                        IsAuthenticated = true;
                        AuthenticatedUser = u;
                        _username = username;
                        _password = password;
                        Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Contexts.Clear();
                                Contexts.Add(new Context() { User = u });
                            }));

                        _client.Organizations.GetOrganizationsAsync(AuthenticatedUser.Login,
                            new Action<IEnumerable<User>>(
                                orgs =>
                                {
                                    Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            foreach (var org in orgs)
                                                Contexts.Add(new Context() { User = org });
                                        }));
                                }),
                            new Action<GitHubException>(
                                ex =>
                                {
                                    MessageBox.Show("Error: " + ex.Message);
                                }));

                        if (onComplete != null)
                            onComplete(IsAuthenticated);
                    }),
                new Action<GitHubException>(
                    ex =>
                    {
                        if (ex.ErrorType == ErrorType.Unauthorized)
                        {
                            Logout();

                            if (onComplete != null)
                                onComplete(IsAuthenticated);
                        }
                    }));
        }

        public void Logout()
        {
            IsAuthenticated = false;
            _username = _password = null;

            AuthenticatedUser = null;
            Contexts.Clear();

            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                iso.DeleteFile(AuthFilename);
                iso.DeleteFile(RepoFilename);
            }
        }

        public void RefreshAllRepos()
        {
            // TODO: Global progress bar
            if (!IsAuthenticated || Dispatcher == null)
                return;

            var exceptionAction = new Action<GitHubException>(
                    ex =>
                    {
                        MessageBox.Show("Error: " + ex.Message, "", MessageBoxButton.OK);
                    });

            foreach (var context in Contexts)
                RefreshContextRepos(context);
        }

        public void RefreshContextRepos(Context context)
        {
            // TODO: Global progress bar
            if (!IsAuthenticated || Dispatcher == null)
                return;

            var exceptionAction = new Action<GitHubException>(
                    ex =>
                    {
                        MessageBox.Show("Error: " + ex.Message, "", MessageBoxButton.OK);
                    });

            _client.Users.GetRepositoriesAsync(context.User.Login,
                    new Action<IEnumerable<Repository>>(
                        repos =>
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                context.MyRepositories.Clear();
                                foreach (var repo in repos)
                                    context.MyRepositories.Add(repo);
                            }));
                        }), exceptionAction);

            _client.Users.GetWatchedRepositoriesAsync(context.User.Login,
                new Action<IEnumerable<Repository>>(
                    repos =>
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            context.WatchedRepositories.Clear();
                            foreach (var repo in repos)
                                context.WatchedRepositories.Add(repo);
                        }));
                    }), exceptionAction);
        }

        public void Save()
        {
            if (!IsAuthenticated)
                return;

            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                SaveAuth(iso);
                SaveRepos(iso);
            }
        }

        private void SaveRepos(IsolatedStorageFile iso)
        {
            using (var stream = iso.OpenFile(RepoFilename, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(RepoFileVersion);

                bw.Write(Contexts.Count);
                foreach (var context in Contexts)
                {
                    bw.Write(context.User.Login);
                    bw.Write(context.MyRepositories.Count);
                    for (int i = 0; i < context.MyRepositories.Count; i++)
                        context.MyRepositories[i].Save(bw);

                    bw.Write(context.WatchedRepositories.Count);
                    for (int i = 0; i < context.WatchedRepositories.Count; i++)
                        context.WatchedRepositories[i].Save(bw);
                }

                bw.Close();
            }
        }

        private void SaveAuth(IsolatedStorageFile isoStore)
        {
            using (var stream = isoStore.OpenFile(AuthFilename, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(AuthFileVersion);
                bw.Write(_username);
                bw.Write(_password);

                bw.Write(Contexts.Count);
                for (int i = 0; i < Contexts.Count; i++)
                    Contexts[i].User.Save(bw);

                bw.Close();
            }
        }

        public void Load()
        {
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                LoadAuth(iso);
                LoadRepos(iso);
            }
        }

        private void LoadRepos(IsolatedStorageFile iso)
        {
            if (!iso.FileExists(RepoFilename))
                return;

            using (var stream = iso.OpenFile(RepoFilename, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(stream))
            {
                var fileVer = br.ReadInt32();
                var numContexts = br.ReadInt32();
                for (int i = 0; i < numContexts; i++)
                {
                    var login = br.ReadString();
                    foreach (var context in Contexts)
                    {
                        if (context.User.Login.Equals(login))
                        {
                            var numMyRepos = br.ReadInt32();
                            context.MyRepositories.Clear();
                            for (int j = 0; j < numMyRepos; j++)
                            {
                                var repo = new Repository();
                                repo.Load(br, fileVer);
                                context.MyRepositories.Add(repo);
                            }

                            var numWatchedRepos = br.ReadInt32();
                            context.WatchedRepositories.Clear();
                            for (int j = 0; j < numWatchedRepos; j++)
                            {
                                var repo = new Repository();
                                repo.Load(br, fileVer);
                                context.WatchedRepositories.Add(repo);
                            }
                            break;
                        }
                    }
                }

                br.Close();
            }
        }

        private void LoadAuth(IsolatedStorageFile isoStore)
        {
            if (isoStore.FileExists(AuthFilename))
            {
                using (var stream = isoStore.OpenFile(AuthFilename, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(stream))
                {
                    var fileVer = br.ReadInt32();
                    _username = br.ReadString();
                    _password = br.ReadString();
                    IsAuthenticated = true;

                    var numContexts = br.ReadInt32();
                    Contexts.Clear();
                    for (int i = 0; i < numContexts; i++)
                    {
                        var usr = new User();
                        usr.Load(br, fileVer);
                        Contexts.Add(new Context() { User = usr });
                    }
                    AuthenticatedUser = Contexts[0].User;

                    br.Close();
                }
            }
            else
            {
                IsAuthenticated = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
