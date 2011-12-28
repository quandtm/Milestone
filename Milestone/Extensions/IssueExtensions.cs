using System;
using System.Collections.ObjectModel;
using System.IO;
using NGitHub.Models;

namespace Milestone.Extensions
{
    public static class IssueExtensions
    {
        public static void Save(this Issue issue, BinaryWriter writer, ref ObservableCollection<Comment> comments)
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
            if (comments == null)
            {
                writer.Write(0);
            }
            else
            {
                writer.Write(comments.Count);
                for (int i = 0; i < comments.Count; i++)
                {
                    var c = comments[i];
                    writer.Write(c.Body ?? "");
                    writer.Write(c.Id);
                    writer.Write(c.GravatarId ?? "");
                    writer.Write(c.User ?? "");
                    writer.Write(c.Url ?? "");
                    writer.Write(c.CreatedAt.ToString());
                    writer.Write(c.UpdatedAt.ToString());
                }
            }
        }

        public static void Load(this Issue issue, BinaryReader reader, int fileVersion, ref ObservableCollection<Comment> comments)
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
            int numComments = reader.ReadInt32();
            issue.Comments = numComments;
            comments.Clear();
            for (int i = 0; i < numComments; i++)
            {
                var c = new Comment();
                c.Body = reader.ReadString();
                c.Id = reader.ReadInt32();
                c.GravatarId = reader.ReadString();
                c.User = reader.ReadString();
                c.Url = reader.ReadString();
                c.CreatedAt = DateTime.Parse(reader.ReadString());
                c.UpdatedAt = DateTime.Parse(reader.ReadString());
                comments.Add(c);
            }
        }
    }
}
