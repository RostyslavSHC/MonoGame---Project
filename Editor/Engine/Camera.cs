using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using System.IO;

// Groups this code inside the Editor project
namespace Editor.Engine
{
    // Creates a camera class
    internal class Camera : ISerializable
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0); // Stores camera position in 3D vector
        public Matrix View { get; set; } = Matrix.Identity; // Stores what camera is looking at (using Matrix)
        public Matrix Projection { get; set; } = Matrix.Identity; // Stores how 3D is shown on screen (using Matrix)
        public float NearPlane { get; set; } = 0.1f; // Closest visible distance
        public float FarPlane { get; set; } = 1000f; // Farthest visible distance
        public float AspectRatio { get; set; } = 16.0f / 9.0f;

        public Camera()
        {
        }

        public Camera(Vector3 _position, float _aspectRatio) // Creates the camera
        {
            Update(_position, _aspectRatio); // Sets up the camera values
        }

        public void Update(Vector3 _position, float _aspectRatio) // Updates the camera
        {
            Position = _position; // Sets the camera position
            AspectRatio = _aspectRatio; // Sets the camera aspect ratio
            View = Matrix.CreateLookAt(Position, // Creates the camera view
                                       new Vector3(0, 0, 0), // Makes the camera look at the center
                                       Vector3.Up); // Defines which direction is up

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), AspectRatio, NearPlane, FarPlane); // Creates the camera projection
        }





        public void Serialize(BinaryWriter _stream) // Serializes the camera
        {
            HelpSerialize.Vec3(_stream, Position);
            _stream.Write(NearPlane); // Serializes X value
            _stream.Write(FarPlane); // Serializes Y value
            _stream.Write(AspectRatio); // Serializes Z value
        }
        public void Deserialize(BinaryReader _stream, ContentManager _content) // Deserializes the camera
        {
            Position = HelpDeserialize.Vec3(_stream); // Deserializes the camera position
            NearPlane = _stream.ReadSingle(); // Deserializes the camera near plane
            FarPlane = _stream.ReadSingle(); // Deserializes the camera far plane
            AspectRatio = _stream.ReadSingle();
            Update(Position, AspectRatio);
        }
    }
}