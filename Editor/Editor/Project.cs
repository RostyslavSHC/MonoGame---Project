using Editor.Engine;


//using Editor.Engine;
using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;

// Groups this code inside the GUI.Editor namespace
namespace Editor.Editor
{
    internal class Project : ISerializable // Represents 1 editor project
    {
        public Level CurrentLevel { get; set; } = null;
        public List<Level> Levels { get; set; } = new();
        public string Folder { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public Project() 
        {
        }

        // Creates a new project
        public Project(ContentManager _content, string _name)
        {
            Folder = Path.GetDirectoryName(_name); // Gets folder path
            Name = Path.GetFileName(_name); // Gets project file name
            if (!Name.ToLower().EndsWith(".oce")) // Checks if file has .oce
            {
                Name += ".oce"; // Adds .oce if missing
            }

            AddLevel(_content); // Creates first level
        }

        public void AddLevel(ContentManager _content) // Adds new level
        {
            CurrentLevel = new(); // Create new level
            CurrentLevel.LoadContent(_content); // Loads level content
            Levels.Add(CurrentLevel); // Adds level to list
        }
        public void Update(float _delta)
        {
            CurrentLevel?.Update(_delta);
        }


        public void Render() // Draws current level
        {
            CurrentLevel.Render(); // Renders current level
        }







        // Serialization
        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(Levels.Count);
            int clIndex = Levels.IndexOf(CurrentLevel);

            foreach (var level in Levels)
            {
                level.Serialize(_stream);
            }

            _stream.Write(clIndex);
            _stream.Write(Folder);
            _stream.Write(Name);
        }

        // Deserialization
        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            int levelCount = _stream.ReadInt32();

            for (int count = 0; count < levelCount; count++)
            {
                Level l = new();
                l.Deserialize(_stream, _content);
                Levels.Add(l);
            }

            int clIndex = _stream.ReadInt32();
            CurrentLevel = Levels[clIndex];
            Folder = _stream.ReadString();
            Name = _stream.ReadString();
        }





    }
}