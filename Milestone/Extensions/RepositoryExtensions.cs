using System.IO;
using NGitHub.Models;

namespace Milestone.Extensions
{
    public static class RepositoryExtensions
    {
        public static void Save(this Repository repo, BinaryWriter writer)
        {
            writer.Write(repo.Name);
            writer.Write(repo.HasIssues);
            writer.Write(repo.Owner);
        }

        public static void Load(this Repository repo, BinaryReader reader, int fileVersion)
        {
            repo.Name = reader.ReadString();
            repo.HasIssues = reader.ReadBoolean();
            repo.Owner = reader.ReadString();
        }
    }
}
