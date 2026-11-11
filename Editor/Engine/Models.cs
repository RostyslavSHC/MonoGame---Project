using Editor.Editor;
using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
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
    class Models : ISerializable, ISelectable
    {
        // Accessors
        public Model Mesh { get; set; } // Stores 3D model
        public Texture Texture { get; set; } // Stores texture placed on model
        public Effect Shader { get; set; } // Stores shader used to draw model
        public Vector3 Position { get => m_position; set { m_position = value; } } // Gets or changes the model position
        public Vector3 Rotation { get => m_rotation; set { m_rotation = value; } } // Gets or changes the model rotation
        public float Scale { get; set; } // Stores the model size

        public bool Selected
        {
            get { return m_selected; }
            set
            {
                if (m_selected != value)
                {
                    m_selected = value;
                    SelectedDirty = true;
                }
            }
        }
        public static bool SelectedDirty { get; set; } = false;

        // Members
        private Vector3 m_position; // Stores position
        private Vector3 m_rotation; // Stores rotation
        private bool m_selected;



        public Models()
        {
        }

        public Models(GameEditor _game,
                      string _model,
                      string _texture,
                      string _effect,
                      Vector3 _position,
                      float _scale) // Receives 3D model, texture, starting position,  starting size
        {
            Create(_game, _model, _texture, _effect, _position, _scale);
            /*
            Mesh = _model; // Saves the model
            Texture = _texture; // Saves the texture
            m_position = _position; // Saves the starting position
            Scale = _scale; // Saves the starting size
            */

        }

        public void Create(GameEditor _game,
                           string _model,
                           string _texture,
                           string _effect,
                           Vector3 _position,
                           float _scale)
        {
            Mesh = _game.Content.Load<Model>(_model);
            Mesh.Tag = _model;

            if (_texture == "DefaultTexture")
            {
                Texture = _game.DefaultTexture;
            }
            else
            {
                Texture = _game.Content.Load<Texture>(_texture);
            }

            Texture.Tag = _texture;

            if (_effect == "DefaultEffect")
            {
                Shader = _game.DefaultEffect;
            }
            else
            {
                Shader = _game.Content.Load<Effect>(_effect);
            }

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






        public void Translate(Vector3 _translate, Camera _camera)
        {
            // Calculate distance between camera and target
            float distance = Vector3.Distance(_camera.Target, _camera.Position);

            // Get the direction the camera is facing
            Vector3 forward = _camera.Target - _camera.Position;
            forward.Normalize();

            // Find the left direction
            Vector3 left = Vector3.Cross(forward, Vector3.Up);
            left.Normalize();

            // Find the up direction
            Vector3 up = Vector3.Cross(left, forward);
            up.Normalize();

            Position += left * _translate.X * distance; // Move model left or right
            Position += up * _translate.Y * distance; // Move model up or down
            Position += forward * _translate.Z * 100f; // Move model forward or backward
        }

        public void Rotate(Vector3 _rotate)
        {
            Rotation += _rotate; // Add rotation to the model
        }






        public Matrix GetTransform() // Creates model's transformation matrix
        {
            return Matrix.CreateScale(Scale) * // Applies model size
                   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * // Applies rotation
                   Matrix.CreateTranslation(Position); // Applies position
        }

        public void Render(Matrix _view, Matrix _projection) // Receives camera view matrix and camera projection matrix
        {
            // m_position.X += 0.001f; // Slowly moves the model on the X axis. I commented it because othervise teapot is tooo much to the right --------------------
            // m_rotation.Y += 0.005f; // Slowly rotates the model on the Y axis

            Shader.Parameters["World"].SetValue(GetTransform()); // Sends model transform to shader
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection); // Sends all rendering matrices to shader
            Shader.Parameters["Texture"].SetValue(Texture); // Sends texture to shader
            Shader.Parameters["Tint"].SetValue(Selected);


            foreach (ModelMesh mesh in Mesh.Meshes) // Loop through every mesh
            {
                mesh.Draw(); // Draws the mesh
            }
        }






        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(Mesh.Tag.ToString());
            _stream.Write(Texture.Tag.ToString());
            _stream.Write(Shader.Tag.ToString());
            HelpSerialize.Vec3(_stream, Position);
            HelpSerialize.Vec3(_stream, Rotation);
            _stream.Write(Scale);
        }

        public void Deserialize(BinaryReader _stream, GameEditor _game)
        {
            string mesh = _stream.ReadString();
            string texture = _stream.ReadString();
            string shader = _stream.ReadString();
            Position = HelpDeserialize.Vec3(_stream);
            Rotation = HelpDeserialize.Vec3(_stream);
            Scale = _stream.ReadSingle();
            Create(_game, mesh, texture, shader, Position, Scale);
        }





    }
}


