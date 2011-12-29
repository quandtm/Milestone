using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Threading;
using Milestone.Extensions;
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
        private const int RepoFileVersion = 2;

        public bool IsAuthenticated { get; private set; }

        private readonly GitHubClient _client;
        public User AuthenticatedUser { get; private set; }
        public ObservableCollection<Context> Contexts { get; private set; }

        public Dispatcher Dispatcher { get; set; }

        private string _username, _password;
        private readonly Action<GitHubException> _exceptionAction;


        public GitHubModel(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            _exceptionAction = ex => Dispatcher.BeginInvoke(() =>
                    MessageBox.Show("Error: " + Enum.GetName(typeof(ErrorType), ex.ErrorType), "", MessageBoxButton.OK)
                );

            _client = new GitHubClient();

            Contexts = new ObservableCollection<Context>();
        }

        public void Login(string username, string password, Action<bool> onComplete)
        {
            // TODO: Global progress bar
            InitAuth(username, password);
            _client.Users.GetAuthenticatedUserAsync(
                new Action<User>(
                    u =>
                    {
                        IsAuthenticated = true;
                        AuthenticatedUser = u;
                        _username = username;
                        _password = password;
                        Dispatcher.BeginInvoke(() =>
                                                   {
                                                       Contexts.Clear();
                                                       Contexts.Add(new Context() { User = u });
                                                   });

                        _client.Organizations.GetOrganizationsAsync(AuthenticatedUser.Login,
                            orgs => Dispatcher.BeginInvoke(() =>
                                                               {
                                                                   foreach (var org in orgs)
                                                                       Contexts.Add(new Context() { User = org });
                                                               }),
                            _exceptionAction);

                        if (onComplete != null)
                            onComplete(IsAuthenticated);
                    }),
                ex =>
                {
                    if (ex.ErrorType == ErrorType.Unauthorized)
                    {
                        Logout();

                        if (onComplete != null)
                            onComplete(IsAuthenticated);
                    }
                });
        }

        private void InitAuth(string username, string password)
        {
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        public void Logout()
        {
            IsAuthenticated = false;
            _username = _password = null;

            _client.Authenticator = null;
            AuthenticatedUser = null;
            Contexts.Clear();

            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                iso.DeleteFile(AuthFilename);
                iso.DeleteFile(RepoFilename);
            }
        }

        public void RefreshAllRepos(Action onStart, Action onComplete)
        {
            if (!IsAuthenticated || Dispatcher == null)
                return;

            foreach (var context in Contexts)
                RefreshContextRepos(context, onStart, onComplete);
        }

        public void RefreshContextRepos(Context context, Action onStart, Action onComplete)
        {
            if (!IsAuthenticated || Dispatcher == null)
                return;

            if (onStart != null)
            {
                // Call twice for two GET operations
                onStart();
                onStart();
            }

            foreach (var repo in context.Repositories)
                repo.Type = RepoType.None;

            _client.Users.GetRepositoriesAsync(context.User.Login,
                    repos => Dispatcher.BeginInvoke(() =>
                                                        {
                                                            foreach (var repo in repos)
                                                            {
                                                                var newRepo = context.Repositories.FirstOrDefault(r => r.Repository.FullName == repo.FullName);
                                                                if (newRepo == null)
                                                                {
                                                                    newRepo = new Repo(repo);
                                                                    newRepo.Type |= RepoType.Owned;
                                                                    context.Repositories.Add(newRepo);
                                                                }
                                                                else
                                                                {
                                                                    newRepo.Repository = repo;
                                                                    newRepo.Type |= RepoType.Owned;
                                                                }
                                                            }
                                                            if (onComplete != null)
                                                                onComplete();
                                                        }), _exceptionAction);

            _client.Users.GetWatchedRepositoriesAsync(context.User.Login,
                repos => Dispatcher.BeginInvoke(() =>
                                                    {
                                                        foreach (var repo in repos)
                                                        {
                                                            var newRepo = context.Repositories.FirstOrDefault(r => r.Repository.FullName == repo.FullName);
                                                            if (newRepo == null)
                                                            {
                                                                newRepo = new Repo(repo);
                                                                newRepo.Type |= RepoType.Watched;
                                                                context.Repositories.Add(newRepo);
                                                            }
                                                            else
                                                            {
                                                                newRepo.Repository = repo;
                                                                newRepo.Type |= RepoType.Watched;
                                                            }
                                                        }
                                                        if (onComplete != null)
                                                            onComplete();
                                                    }), _exceptionAction);
        }

        public void DownloadIssues(Context context, Repo r, Action onStart, Action onComplete)
        {
            if (onStart != null)
            {
                // Call twice for two GET operations
                onStart();
                onStart();
            }
            // Download open issues
            _client.Issues.GetIssuesAsync(r.Repository.Owner, r.Repository.Name, State.Open,
                issues => Dispatcher.BeginInvoke(() =>
                                                     {
                                                         foreach (var i in issues)
                                                         {
                                                             var oldIssue = r.Issues.FirstOrDefault(iss => iss.Number == i.Number);
                                                             if (oldIssue != null)
                                                                 r.Issues.Remove(oldIssue);
                                                             r.Issues.Add(i);
                                                         }
                                                         if (onComplete != null)
                                                             onComplete();
                                                     }),
                _exceptionAction);

            // Download closed issues
            _client.Issues.GetIssuesAsync(r.Repository.Owner, r.Repository.Name, State.Closed,
                issues => Dispatcher.BeginInvoke(() =>
                                                    {
                                                        foreach (var i in issues)
                                                        {
                                                            var oldIssue = r.Issues.FirstOrDefault(iss => iss.Number == i.Number);
                                                            if (oldIssue != null)
                                                                r.Issues.Remove(oldIssue);
                                                            r.Issues.Add(i);
                                                        }
                                                        if (onComplete != null)
                                                            onComplete();
                                                    }),
                _exceptionAction);
        }

        public void DownloadIssueComments(Context context, Repo r, Issue i, Action<Repo> callback)
        {
            //_client.Issues.
            _client.Issues.GetCommentsAsync(r.Repository.Owner, r.Repository.Name, i.Number,
                                            comments => Dispatcher.BeginInvoke(() =>
                                                                                   {
                                                                                       if (r.IssueComments.ContainsKey(i))
                                                                                           r.IssueComments[i].Clear();
                                                                                       else
                                                                                           r.IssueComments.Add(i, new ObservableCollection<Comment>());

                                                                                       foreach (var c in comments)
                                                                                       {
                                                                                           r.IssueComments[i].Add(c);
                                                                                       }

                                                                                       callback(r);
                                                                                   }),
                                            _exceptionAction);
        }

        public void CreateComment(string repoOwner, string repoName, int issueNumber, string comment, Action onStart, Action onComplete)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return;
            if (onStart != null)
                onStart();
            _client.Issues.CreateCommentAsync(repoOwner, repoName, issueNumber, comment,
                (c) =>
                {
                    var context = Contexts.Select(x => x.Repositories.FirstOrDefault(r => r.Repository.Name == repoName && r.Repository.Owner == repoOwner)).FirstOrDefault();
                    if (context != null)
                    {
                        var issue = context.Issues.FirstOrDefault(i => i.Number == issueNumber);

                        ObservableCollection<Comment> comments;
                        if (issue != null && context.IssueComments.TryGetValue(issue, out comments))
                        {
                            Dispatcher.BeginInvoke(() => comments.Add(c));
                        }
                    }

                    if (onComplete != null)
                        onComplete();
                },
                _exceptionAction);
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
                    int numRepos = 0;
                    foreach (var repo in context.Repositories)
                    {
                        if (repo.Type != RepoType.None)
                            ++numRepos;
                    }
                    bw.Write(numRepos);
                    for (int i = 0; i < context.Repositories.Count; i++)
                    {
                        if (context.Repositories[i].Type != RepoType.None)
                            context.Repositories[i].Save(bw);
                    }
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
                byte[] unamePlainData = System.Text.Encoding.UTF8.GetBytes(_username);
                byte[] pwordPlainData = System.Text.Encoding.UTF8.GetBytes(_password);
                byte[] unameEncrypted = ProtectedData.Protect(unamePlainData, null);
                byte[] pwordEncrypted = ProtectedData.Protect(pwordPlainData, null);
                bw.Write(unameEncrypted.Length);
                bw.Write(unameEncrypted, 0, unameEncrypted.Length);
                bw.Write(pwordEncrypted.Length);
                bw.Write(pwordEncrypted, 0, pwordEncrypted.Length);

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

            try
            {
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
                                context.Repositories.Clear();
                                for (int j = 0; j < numMyRepos; j++)
                                {
                                    var repo = new Repo();
                                    repo.Load(br, fileVer);
                                    context.Repositories.Add(repo);
                                }
                                break;
                            }
                        }
                    }

                    br.Close();
                }
            }
            catch (EndOfStreamException)
            {
                iso.DeleteFile(RepoFilename);
            }
        }

        private void LoadAuth(IsolatedStorageFile isoStore)
        {
            if (isoStore.FileExists(AuthFilename))
            {
                try
                {
                    using (var stream = isoStore.OpenFile(AuthFilename, FileMode.Open, FileAccess.Read))
                    using (var br = new BinaryReader(stream))
                    {
                        var fileVer = br.ReadInt32();
                        var unameLen = br.ReadInt32();
                        var unameEncrypted = br.ReadBytes(unameLen);
                        var pwordLen = br.ReadInt32();
                        var pwordEncrypted = br.ReadBytes(pwordLen);
                        var unamePlain = ProtectedData.Unprotect(unameEncrypted, null);
                        var pwordPlain = ProtectedData.Unprotect(pwordEncrypted, null);
                        _username = System.Text.Encoding.UTF8.GetString(unamePlain, 0, unamePlain.Length);
                        _password = System.Text.Encoding.UTF8.GetString(pwordPlain, 0, pwordPlain.Length);
                        IsAuthenticated = true;
                        InitAuth(_username, _password);

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
                catch (EndOfStreamException)
                {
                    isoStore.DeleteFile(AuthFilename);
                }
            }
            else
            {
                IsAuthenticated = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UploadIssue(Repo r, string title, string body, Action<Issue> callback)
        {
            _client.Issues.CreateIssueAsync(r.Repository.Owner,
                                            r.Repository.Name,
                                            title,
                                            body,
                                            callback,
                                            _exceptionAction);
        }
    }
}
