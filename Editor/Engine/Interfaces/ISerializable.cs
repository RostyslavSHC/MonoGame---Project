using Editor.Editor;
using Microsoft.Xna.Framework.Content;
using System.IO;

// Groups this interface inside the Interfaces folder
namespace Editor.Engine.Interfaces
{
    // Defines rules for saving and loading objects
    internal interface ISerializable
    {
        public void Serialize(BinaryWriter _stream); // Saves object data into a binary file
        public void Deserialize(BinaryReader _stream, GameEditor _game); // Loads object data from a binary file
    }
}