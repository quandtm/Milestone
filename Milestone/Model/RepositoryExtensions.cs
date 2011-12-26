using System.IO;
using NGitHub.Models;

namespace Milestone.Model
{
    public static class RepositoryExtensions
    {
        public static void Save(this Repository repo, BinaryWriter writer)
        {
            writer.Write(repo.Name);
            writer.Write(repo.HasIssues);
            writer.Write(repo.Description);
            writer.Write(repo.NumberOfForks);
        }

        public static void Load(this Repository repo, BinaryReader reader, int fileVersion)
        {
            repo.Name = reader.ReadString();
            repo.HasIssues = reader.ReadBoolean();
            repo.Description = reader.ReadString();
            repo.NumberOfForks = reader.ReadInt32();
        }
    }
}
