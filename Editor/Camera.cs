using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

// Groups this code inside the Editor project
namespace Editor
{
    // Creates a camera class
    internal class Camera
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0); // Stores camera position in 3D vector
        public Matrix View { get; set; } = Matrix.Identity; // Stores what camera is looking at (using Matrix)
        public Matrix Projection { get; set; } = Matrix.Identity; // Stores how 3D is shown on screen (using Matrix)
        public float NearPlane { get; set; } = 0.1f; // Closest visible distance
        public float FarPlane { get; set; } = 1000f; // Farthest visible distance

        public Camera(Vector3 _position, float _aspectRatio) // Creates the camera
        {
            Update(_position, _aspectRatio); // Sets up the camera values
        }

        public void Update(Vector3 _position, float _aspectRatio) // Updates the camera
        {
            Position = _position; // Sets the camera position
            View = Matrix.CreateLookAt(Position, // Creates the camera view
                                       new Vector3(0, 0, 0), // Makes the camera look at the center
                                       Vector3.Up); // Defines which direction is up

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), _aspectRatio, NearPlane, FarPlane); // Creates the camera projection
        }
    }
}