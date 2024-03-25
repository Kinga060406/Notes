using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Notes.Models
{
    internal class Note
    {
        public string Filename { get; set; }
        public string Text { get; set; }
        public List<GroupMember> Members { get; set; }

        public Note()
        {
            Filename = $"{Path.GetRandomFileName()}.notes.txt";
            Text = "";
            Members = new List<GroupMember>();
        }

        public void Save()
        {
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, Filename), Text);
            // Additionally, save group members if needed
            // For simplicity, assume members are saved to a separate file or database
        }

        public void Delete()
        {
            File.Delete(Path.Combine(FileSystem.AppDataDirectory, Filename));
            // Additionally, delete group members if needed
            // For simplicity, assume members are deleted from a separate file or database
        }

        public static Note Load(string filename)
        {
            filename = Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(filename))
                throw new FileNotFoundException("Unable to find file on local storage.", filename);

            // Additionally, load group members if needed
            // For simplicity, assume members are loaded from a separate file or database

            return new Note
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
                Members = new List<GroupMember>() // Initialize with an empty list for now
            };
        }

        public static IEnumerable<Note> LoadAll()
        {
            string appDataPath = FileSystem.AppDataDirectory;

            return Directory
                .EnumerateFiles(appDataPath, "*.notes.txt")
                .Select(filename => Load(Path.GetFileName(filename)));
        }

        // Method to add a member to the group
        public void AddMember(GroupMember member)
        {
            Members.Add(member);
            // Additionally, save members if needed
        }

        // Method to remove a member from the group
        public void RemoveMember(GroupMember member)
        {
            Members.Remove(member);
            // Additionally, delete member if needed
        }
    }

    // Class representing a member of a group
    internal class GroupMember
    {
        public string Name { get; set; }
        // Add any other properties of a group member as needed
    }
}
