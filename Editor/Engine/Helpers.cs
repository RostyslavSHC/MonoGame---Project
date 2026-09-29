using Microsoft.Xna.Framework;
using System.IO;

// Groups these helper classes inside the Engine namespace
namespace Editor.Engine
{
    // Helper class for saving data
    internal class HelpSerialize
    {
        public static void Vec3(BinaryWriter _stream, Vector3 _vector) // Saves a Vector3
        {
            _stream.Write(_vector.X); // Saves X value
            _stream.Write(_vector.Y); // Saves Y value
            _stream.Write(_vector.Z); // Saves Z value
        }
    }

    // Helper class for loading data
    internal class HelpDeserialize
    {
        public static Vector3 Vec3(BinaryReader _stream) // Loads a Vector3
        {
            Vector3 v = Vector3.Zero; // Creates an empty Vector3
            v.X = _stream.ReadSingle(); // Loads X value
            v.Y = _stream.ReadSingle(); // Loads Y value
            v.Z = _stream.ReadSingle(); // Loads Z value
            return v; // Returns the loaded Vector3
        }
    }
}