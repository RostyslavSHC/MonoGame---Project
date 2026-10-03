using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Groups this code inside the Editor project
namespace Editor.Engine
{
    // Represents and controls a 3D model
    class Models : ISerializable
    {
        // Accessors
        public Model Mesh { get; set; } // Stores 3D model
        public Effect Shader { get; set; } // Stores shader used to draw model
        public Vector3 Position { get => m_position; set { m_position = value; } } // Gets or changes the model position
        public Vector3 Rotation { get => m_rotation; set { m_rotation = value; } } // Gets or changes the model rotation
        public float Scale { get; set; } // Stores the model size

        // NEW 
        public string Name { get; set; } // Stores 3D model name
        public float RotationSpeed { get; set; } = 0.005f; // Speed at which the model rotates
        public float OrbitSpeed { get; set; } = 0.0f; // Speed at which the model orbits around a point
        public Models Parent { get; set; } = null; // Parent model because Moon needs to know to which planet it belongs



        // Texturing
        public Texture Texture { get; set; } // Stores texture placed on model

        // Members
        private Vector3 m_position; // Stores position
        private Vector3 m_rotation; // Stores rotation

        private float m_orbitAngle = 0.0f; // NEW ----- Angle for orbiting around a point

        public Models()
        {
        }

        public Models(ContentManager _content,
                      string _model,
                      string _texture,
                      string _effect,
                      Vector3 _position,
                      float _scale) // Receives 3D model, texture, starting position,  starting size
        {
            Create(_content, _model, _texture, _effect, _position, _scale);
            /*
            Mesh = _model; // Saves the model
            Texture = _texture; // Saves the texture
            m_position = _position; // Saves the starting position
            Scale = _scale; // Saves the starting size
            */

        }

        public void Create(ContentManager _content,
                           string _model,
                           string _texture,
                           string _effect,
                           Vector3 _position,
                           float _scale)
        {
            Name = _model; // NEW ----- Saves the model name
            Mesh = _content.Load<Model>(_model);
            Mesh.Tag = _model;
            Texture = _content.Load<Texture>(_texture);
            Texture.Tag = _texture;
            Shader = _content.Load<Effect>(_effect);
            Shader.Tag = _effect;
            SetShader(Shader);
            m_position = _position;
            Scale = _scale;
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
            // NEW
            m_rotation.Y += RotationSpeed; // NEW ----- Slowly rotates the model on the Y axis based on RotationSpeed

            if (Parent != null) // If the model has a parent, it orbits around the parent
            {
                // Creates an offset for orbiting around the parent
                Vector3 offset = Vector3.Transform(new Vector3(20, 0, 0), Matrix.CreateRotationY(m_orbitAngle));

                m_position = Parent.Position + offset; // Sets the model position to be the parent position plus the offset
                m_orbitAngle += OrbitSpeed; // Increases the orbit angle based on OrbitSpeed
            }
            
            // Orbit around the centre of a Sun (0, 0, 0). If there is no parent, the model orbits around the centre of the level
            else if (OrbitSpeed != 0.0f)
            {
                m_position = Vector3.Transform(m_position, Matrix.CreateRotationY(OrbitSpeed)); // Slowly orbits the model around the Y axis based on OrbitSpeed
            }





            Shader.Parameters["World"].SetValue(GetTransform()); // Sends model transform to shader
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection); // Sends all rendering matrices to shader
            Shader.Parameters["Texture"].SetValue(Texture); // Sends texture to shader

            foreach (ModelMesh mesh in Mesh.Meshes) // Loop through every mesh
            {
                mesh.Draw(); // Draws the mesh
            }
        }





        // Serialization - Saves the model
        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(Mesh.Tag.ToString());
            _stream.Write(Texture.Tag.ToString());
            _stream.Write(Shader.Tag.ToString());
            HelpSerialize.Vec3(_stream, Position);
            HelpSerialize.Vec3(_stream, Rotation);
            _stream.Write(Scale);

            // NEW ----- Save movement and rotation speeds
            _stream.Write(RotationSpeed); // Save rotation speed
            _stream.Write(OrbitSpeed); // Save orbit speed
            _stream.Write(m_orbitAngle); // Save orbit angle
        }

        // Deserialization - Loads the model
        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            string mesh = _stream.ReadString();
            string texture = _stream.ReadString();
            string shader = _stream.ReadString();
            Position = HelpDeserialize.Vec3(_stream);
            Rotation = HelpDeserialize.Vec3(_stream);
            Scale = _stream.ReadSingle();

            // NEW ----- Load movement and rotation speeds
            RotationSpeed = _stream.ReadSingle(); // Load rotation speed
            OrbitSpeed = _stream.ReadSingle(); // Load orbit speed
            m_orbitAngle = _stream.ReadSingle(); // Load orbit angle

            Create(_content, mesh, texture, shader, Position, Scale);

        }





    }
}


