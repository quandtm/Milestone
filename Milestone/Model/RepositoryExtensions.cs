using System.IO;
using NGitHub.Models;

namespace Milestone.Model
{
    public static class RepositoryExtensions
    {
        public static void Save(this Repository repo, BinaryWriter writer)
        {
            writer.Write(repo.Name);
        }

        public static void Load(this Repository repo, BinaryReader reader, int fileVersion)
        {
            repo.Name = reader.ReadString();
        }
    }
}
