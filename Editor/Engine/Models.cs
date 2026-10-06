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
        public Model Mesh { get; set; }
        public Effect Shader { get; set; }
        public Vector3 Position { get => m_position; set { m_position = value; } } // Gets or changes the model position
        public Vector3 Rotation { get => m_rotation; set { m_rotation = value; } } // Gets or changes the model rotation
        public float Scale { get; set; }

        public string Name { get; set; }
        public float RotationSpeed { get; set; } = 0.005f;
        public float OrbitSpeed { get; set; } = 0.0f;
        public Models Parent { get; set; } = null; // Parent model because Moon needs to know to which planet it belongs

        // Texturing
        public Texture Texture { get; set; }

        // Members
        private Vector3 m_position;
        private Vector3 m_rotation;
        private float m_orbitAngle = 0.0f;

        public Models()
        {
        }

        // Parameterized constructor
        public Models(ContentManager _content,
                      string _model,
                      string _texture,
                      string _effect,
                      Vector3 _position,
                      float _scale)
        {
            Create(_content, _model, _texture, _effect, _position, _scale); // Creates the model
        }

        // Creates the model
        public void Create(ContentManager _content,
                           string _model,
                           string _texture,
                           string _effect,
                           Vector3 _position,
                           float _scale)
        {
            Name = _model;
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


        // Sets the shader for the model
        public void SetShader(Effect _effect)
        {
            Shader = _effect; // Saves shader
            foreach (ModelMesh mesh in Mesh.Meshes) // Loops through every mesh
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts) // Loops through every mesh part
                {
                    meshPart.Effect = Shader; // Applies shader to that mesh part
                }
            }
        }

        // Creates model's transformation matrix (position, rotation, scale)
        public Matrix GetTransform()
        {
            return Matrix.CreateScale(Scale) *
                   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                   Matrix.CreateTranslation(Position);
        }

        // Receives camera view matrix and camera projection matrix
        public void Render(Matrix _view, Matrix _projection)
        {
            m_rotation.Y += RotationSpeed;

            if (Parent != null) // If the model has a parent, it orbits around the parent
            {
                Vector3 offset = Vector3.Transform(new Vector3(20, 0, 0), Matrix.CreateRotationY(m_orbitAngle)); // Creates an offset based on the orbit angle

                m_position = Parent.Position + offset;
                m_orbitAngle += OrbitSpeed;
            }
            
            // Orbit around the centre of a Sun (0, 0, 0). If there is no parent, the model orbits around the centre of the level
            else if (OrbitSpeed != 0.0f)
            {
                m_position = Vector3.Transform(m_position, Matrix.CreateRotationY(OrbitSpeed)); // Slowly orbits the model around the Y axis based on OrbitSpeed
            }

            // Sends the model's data to the shader
            Shader.Parameters["World"].SetValue(GetTransform());
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
            Shader.Parameters["Texture"].SetValue(Texture);

            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                mesh.Draw();
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

            // Save movement and rotation speeds
            _stream.Write(RotationSpeed);
            _stream.Write(OrbitSpeed);
            _stream.Write(m_orbitAngle);
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

            // Load movement and rotation speeds
            RotationSpeed = _stream.ReadSingle();
            OrbitSpeed = _stream.ReadSingle();
            m_orbitAngle = _stream.ReadSingle();

            Create(_content, mesh, texture, shader, Position, Scale); // Creates the model

        }





    }
}


