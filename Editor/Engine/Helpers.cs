using Microsoft.Xna.Framework;
using System.IO;

namespace Editor.Engine
{
    // Helper class for saving data
    internal class HelpSerialize
    {
        public static void Vec3(BinaryWriter _stream, Vector3 _vector) // Saves a Vector3
        {
            _stream.Write(_vector.X);
            _stream.Write(_vector.Y);
            _stream.Write(_vector.Z);
        }
    }

    // Helper class for loading data
    internal class HelpDeserialize
    {
        public static Vector3 Vec3(BinaryReader _stream) // Loads a Vector3
        {
            Vector3 v = Vector3.Zero;
            v.X = _stream.ReadSingle();
            v.Y = _stream.ReadSingle();
            v.Z = _stream.ReadSingle();
            return v;
        }
    }
}