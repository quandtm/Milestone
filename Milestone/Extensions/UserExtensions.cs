using System.IO;
using NGitHub.Models;

namespace Milestone.Extensions
{
    public static class UserExtensions
    {
        public static void Save(this User user, BinaryWriter writer)
        {
            writer.Write(user.Login);
        }

        public static void Load(this User user, BinaryReader reader, int fileVersion)
        {
            user.Login = reader.ReadString();
        }
    }
}
