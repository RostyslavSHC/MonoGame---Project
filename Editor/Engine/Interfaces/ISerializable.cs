using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Editor.Engine.Interfaces
{
    // Defines rules for saving and loading objects
    internal interface ISerializable
    {
        public void Serialize(BinaryWriter _stream); // Saves object data into a binary file
        public void Deserialize(BinaryReader _stream, ContentManager _content); // Loads object data from a binary file
    }
}