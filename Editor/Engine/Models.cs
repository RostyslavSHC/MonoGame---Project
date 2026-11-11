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
    class Models : ISerializable, ISelectable, IMaterial
    {
        // Accessors
        public Model Mesh { get; set; } // Stores 3D model
        public Material Material { get; private set; }
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
            Material = new Material();
            SetTexture(_game, _texture);
            SetShader(_game, _effect);
            m_position = _position;
            Scale = _scale;
        }

        public void SetShader(Effect _effect)
        {
            Material.Effect = _effect;
            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Material.Effect;
                }
            }
        }




        public void SetTexture(GameEditor _game, string _texture)
        {
            if (_texture == "DefaultTexture")
            {
                Material.Diffuse = _game.DefaultTexture;
            }
            else
            {
                Material.Diffuse = _game.Content.Load<Texture>(_texture);
            }
            Material.Diffuse.Tag = _texture;
        }

        public void SetShader(GameEditor _game, string _effect)
        {
            if (_effect == "DefaultEffect")
            {
                Material.Effect = _game.DefaultEffect;
            }
            else
            {
                Material.Effect = _game.Content.Load<Effect>(_effect);
            }
            Material.Effect.Tag = _effect;
            SetShader(Material.Effect);
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

        public void Render(Camera _camera)
        {
            Material.Effect.Parameters["World"]?.SetValue(GetTransform());
            Material.Effect.Parameters["WorldViewProjection"]?.SetValue(GetTransform() *
                                                                      _camera.View *
                                                                      _camera.Projection);
            Material.Effect.Parameters["Texture"]?.SetValue(Material.Diffuse);
            Material.Effect.Parameters["Tint"]?.SetValue(Selected);
            Material.Effect.Parameters["CameraPosition"]?.SetValue(_camera.Position);
            Material.Effect.Parameters["View"]?.SetValue(_camera.View);
            Material.Effect.Parameters["Projection"]?.SetValue(_camera.Projection);
            Material.Effect.Parameters["TextureTiling"]?.SetValue(15.0f);
            Material.Effect.Parameters["LightDirection"]?.SetValue(Vector3.One);

            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                mesh.Draw();
            }
        }






        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(Mesh.Tag.ToString());
            _stream.Write(Material.Diffuse.Tag.ToString());
            _stream.Write(Material.Effect.Tag.ToString());
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
            Material = new Material();
            Create(_game, mesh, texture, shader, Position, Scale);
        }





    }
}


