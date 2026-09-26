using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Groups this code inside the Editor project
namespace Editor
{
    // Represents and controls a 3D model
    class Models
    {
        // Accessors
        public Model Mesh { get; set; } // Stores 3D model
        public Effect Shader { get; set; } // Stores shader used to draw model
        public Vector3 Position { get => m_position; set { m_position = value; } } // Gets or changes the model position
        public Vector3 Rotation { get => m_rotation; set { m_rotation = value; } } // Gets or changes the model rotation
        public float Scale { get; set; } // Stores the model size

        // Texturing
        public Texture Texture { get; set; } // Stores texture placed on model

        // Members
        private Vector3 m_position; // Stores position
        private Vector3 m_rotation; // Stores rotation

        public Models(Model _model, Texture _texture, Vector3 _position, float _scale) // Receives 3D model, texture, starting position,  starting size
        {
            Mesh = _model; // Saves the model
            Texture = _texture; // Saves the texture
            m_position = _position; // Saves the starting position
            Scale = _scale; // Saves the starting size
        }

        public void SetShader(Effect _effect) // Sets shader for model
        {
            Shader = _effect; // Saves shader
            foreach (ModelMesh mesh in Mesh.Meshes) // Goes through every mesh in model
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts) // Loops through every mesh part
                {
                    meshPart.Effect = Shader; // Applies shader to that mesh part
                }
            }
        }

        public Matrix GetTransform() // Creates model's transformation matrix
        {
            return Matrix.CreateScale(Scale) * // Applies model size
                   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * // Applies rotation
                   Matrix.CreateTranslation(Position); // Applies position
        }

        public void Render(Matrix _view, Matrix _projection) // Receives camera view matrix and camera projection matrix
        {
            m_position.X += 0.001f; // Slowly moves the model on the X axis
            m_rotation.Y += 0.005f; // Slowly rotates the model on the Y axis

            Shader.Parameters["World"].SetValue(GetTransform()); // Sends model transform to shader
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection); // Sends all rendering matrices to shader
            Shader.Parameters["Texture"].SetValue(Texture); // Sends texture to shader

            foreach (ModelMesh mesh in Mesh.Meshes) // Loop through every mesh
            {
                mesh.Draw(); // Draws the mesh
            }
        }
    }
}