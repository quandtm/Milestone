using System;
using System.IO;
using NGitHub.Models;

namespace Milestone.Extensions
{
    public static class IssueExtensions
    {
        public static void Save(this Issue issue, BinaryWriter writer)
        {
            writer.Write(issue.Title);
            writer.Write(issue.Body);
            writer.Write(issue.User ?? "");
            writer.Write(issue.Number);
            writer.Write(issue.CreatedAt.ToString());
            writer.Write(issue.Labels != null);
            if (issue.Labels != null)
            {
                writer.Write(issue.Labels.Count);
                for (int i = 0; i < issue.Labels.Count; i++)
                    writer.Write(issue.Labels[i].Name);
            }
            writer.Write(issue.GravatarId ?? "");
            writer.Write(issue.State);
        }

        public static void Load(this Issue issue, BinaryReader reader, int fileVersion)
        {
            issue.Title = reader.ReadString();
            issue.Body = reader.ReadString();
            issue.User = reader.ReadString();
            issue.Number = reader.ReadInt32();
            issue.CreatedAt = DateTime.Parse(reader.ReadString());
            var hasLabels = reader.ReadBoolean();
            if (hasLabels)
            {
                var numLabels = reader.ReadInt32();
                issue.Labels.Clear();
                for (int i = 0; i < numLabels; i++)
                {
                    var label = new Label();
                    label.Name = reader.ReadString();
                    issue.Labels.Add(label);
                }
            }
            issue.GravatarId = reader.ReadString();
            issue.State = reader.ReadString();
        }
    }
}
