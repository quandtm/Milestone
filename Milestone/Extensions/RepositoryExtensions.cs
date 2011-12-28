using System.Collections.ObjectModel;
using System.IO;
using Milestone.Model;
using NGitHub.Models;

namespace Milestone.Extensions
{
    public static class RepositoryExtensions
    {
        public static void Save(this Repo repo, BinaryWriter writer)
        {
            writer.Write(repo.Repository.Name);
            writer.Write((short)repo.Type);
            writer.Write(repo.Repository.HasIssues);
            writer.Write(repo.Repository.Owner);
            writer.Write(repo.Issues.Count);
            writer.Write(repo.Repository.IsFork);
            for (int i = 0; i < repo.Issues.Count; i++)
            {
                ObservableCollection<Comment> comments = null;
                repo.IssueComments.TryGetValue(repo.Issues[i], out comments);
                repo.Issues[i].Save(writer, ref comments);
            }
        }

        public static void Load(this Repo repo, BinaryReader reader, int fileVersion)
        {
            repo.Repository = new Repository();
            repo.Repository.Name = reader.ReadString();
            repo.Type = (RepoType)reader.ReadInt16();
            repo.Repository.HasIssues = reader.ReadBoolean();
            repo.Repository.Owner = reader.ReadString();
            var numIssues = reader.ReadInt32();
            repo.Repository.IsFork = reader.ReadBoolean();
            for (int i = 0; i < numIssues; i++)
            {
                var issue = new Issue();
                var oc = new ObservableCollection<Comment>();
                issue.Load(reader, fileVersion, ref oc);
                repo.Issues.Add(issue);
                repo.IssueComments.Add(issue, oc);

            }
        }
    }
}
