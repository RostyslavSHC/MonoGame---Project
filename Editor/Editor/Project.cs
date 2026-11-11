using Editor.Engine;
using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

// Groups this code inside the GUI.Editor namespace
namespace Editor.Editor
{
    internal class Project : ISerializable
    {
        public event AssetsUpdated OnAssetsUpdated;

        public Level CurrentLevel { get; private set; } = null;
        public List<Level> Levels { get; private set; } = new();
        public string Folder { get; private set; } = string.Empty;
        public string ContentFolder { get; private set; } = string.Empty;
        public string AssetFolder { get; private set; } = string.Empty;
        public string ObjectFolder { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public AssetMonitor AssetMonitor { get; private set; } = null;

        public Project()
        {
        }

        // Creates a new project
        public Project(GraphicsDevice _device, ContentManager _content, string _name)
        {
            Folder = Path.GetDirectoryName(_name); // Gets folder path
            Name = Path.GetFileName(_name); // Gets project file name
            if (!Name.ToLower().EndsWith(".oce")) // Checks if file has .oce
            {
                Name += ".oce"; // Adds .oce if missing
            }



            // Create Content folder for assets, and copy the mgcb template
            ContentFolder = Path.Combine(Folder, "Content");
            AssetFolder = Path.Combine(ContentFolder, "bin");
            // ObjectFolder = Path.Combine(ContentFolder, "obj"); --------------------------------------------
            ObjectFolder = Path.Combine(ContentFolder, "obj", "Windows");
            char d = Path.DirectorySeparatorChar;

            if (!Directory.Exists(ContentFolder))
            {
                Directory.CreateDirectory(ContentFolder);
                Directory.CreateDirectory(AssetFolder);
                Directory.CreateDirectory(ObjectFolder);
                File.Copy($"ContentTemplate.mgcb", ContentFolder + $"{d}Content.mgcb");
            }

            AssetMonitor = new(ObjectFolder);
            AssetMonitor.OnAssetsUpdated += AssetMon_OnAssetsUpdated;


            // Add a default level
            AddLevel(_device, _content); // Creates first level
        }

        private void AssetMon_OnAssetsUpdated()
        {
            OnAssetsUpdated?.Invoke();
        }





        public void AddLevel(GraphicsDevice _device, ContentManager _content) // Adds new level
        {
            CurrentLevel = new(); // Create new level
            CurrentLevel.LoadContent(_device, _content); // Loads level content
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