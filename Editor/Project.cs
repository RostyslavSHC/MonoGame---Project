using Editor;
//using Editor.Engine;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;

// Groups this code inside the GUI.Editor namespace
namespace GUI.Editor
{
    internal class Project // Represents 1 editor project
    {
        public Level CurrentLevel { get; set; } = null; // Stores current active level
        public List<Level> Levels { get; set; } = new(); // Stores all levels in project
        public string Folder { get; set; } = string.Empty; // Stores project folder
        public string Name { get; set; } = string.Empty; // Stores project name

        // Creates a new project
        public Project(ContentManager _content, string _name)
        {
            Folder = Path.GetDirectoryName(_name); // Gets folder path
            Name = Path.GetFileName(_name); // Gets project file name
            if (!Name.ToLower().EndsWith(".oce")) // Checks if file has .oce
            {
                Name += ".oce"; // Adds .oce if missing
            }

            // Add a default level
            AddLevel(_content); // Creates first level
        }

        public void AddLevel(ContentManager _content) // Adds new level
        {
            CurrentLevel = new(); // Create new level
            CurrentLevel.LoadContent(_content); // Loads level content
            Levels.Add(CurrentLevel); // Adds level to list
        }

        public void Render() // Draws current level
        {
            CurrentLevel.Render(); // Renders current level
        }
    }
}