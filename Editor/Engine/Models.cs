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
using System.ComponentModel;

// Groups this code inside the Editor project
namespace Editor.Engine
{
    // Represents and controls a 3D model
    [DefaultProperty("Position")]
    class Models : ISerializable, INotifyPropertyChanged
    {

        // Property Changed
        public event PropertyChangedEventHandler PropertyChanged; // Event triggered when a property changes

        protected void OnPropertyChanged(string _propertyName) // Notifies when a property changes
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_propertyName)); // ?. means the event is called only if something is listening to it
        }


        // Accessors
        [Browsable(false)]
        public Model Mesh { get; set; }

        [Browsable(false)]
        public Effect Shader { get; set; } // Stores shader used to draw model


        [Category("Transformation")]
        [Description("Position of the model in world space.")]
        public Vector3 Position // Gets or changes the model position
        {
            get => m_position;
            set
            {
                if (m_position != value) // Check if position changed and notify PropertyGrid
                {
                    m_position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

        [Category("Transformation")]
        [Description("Rotation of the model.")]
        public Vector3 Rotation // Gets or changes the model rotation
        {
            get => m_rotation;
            set
            {
                if (m_rotation != value) // Check if rotation changed, if so save it and notify PropertyGrid
                {
                    m_rotation = value;
                    OnPropertyChanged(nameof(Rotation));
                }
            }
        }

        [Category("Transformation")]
        [Description("Scale of the model.")]
        public float Scale // Gets or changes the model size
        {
            get => m_scale;
            set
            {
                if (m_scale != value) // Check if scale changed, if so save it and notify PropertyGrid
                {
                    m_scale = value;
                    OnPropertyChanged(nameof(Scale));
                }
            }
        }

        [Category("State")]
        [Description("Selection status.")]
        public bool Selected // Gets or changes the selection status
        {
            get => m_selected;
            set
            {
                if (m_selected != value) // Check if selection changed, if so save it and notify PropertyGrid
                {
                    m_selected = value;
                    OnPropertyChanged(nameof(Selected));
                }
            }
        }



        // Texturing
        [Browsable(false)]
        public Texture Texture { get; set; }

        [Category("Appearance")]
        [Description("Diffuse texture of the model.")]
        [TypeConverter(typeof(DiffuseTextureConverter))]
        public string DiffuseTexture // Gets or changes the model texture
        {
            get => m_diffuseTexture;
            set
            {
                if (m_diffuseTexture != value)
                {
                    if (m_content != null)
                    {
                        Texture newTexture = m_content.Load<Texture>(value);
                        newTexture.Tag = value;
                        Texture = newTexture;
                    }

                    m_diffuseTexture = value; // Save selected texture name

                    OnPropertyChanged(nameof(DiffuseTexture)); // Notify listeners
                }
            }
        }

        // Members
        private Vector3 m_position;
        private Vector3 m_rotation;
        private float m_scale;
        private bool m_selected = false;
        private ContentManager m_content;
        private string m_diffuseTexture = "Metal";











        public Models()
        {
        }

        public Models(ContentManager _content,
                      string _model,
                      string _texture,
                      string _effect,
                      Vector3 _position,
                      float _scale) // Receives 3D model, texture, starting position, starting size
        {
            Create(_content, _model, _texture, _effect, _position, _scale);

        }

        public void Create(ContentManager _content,
                           string _model,
                           string _texture,
                           string _effect,
                           Vector3 _position,
                           float _scale)
        {
            m_content = _content;

            Mesh = _content.Load<Model>(_model);
            Mesh.Tag = _model;

            m_diffuseTexture = _texture; // Store initial texture name
            Texture = _content.Load<Texture>(_texture);
            Texture.Tag = _texture;



            Shader = _content.Load<Effect>(_effect);
            Shader.Tag = _effect;
            SetShader(Shader);
            Position = _position;
            Scale = _scale;
        }



        public void SetShader(Effect _effect) // Sets shader for model
        {
            Shader = _effect; // Saves shader
            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Shader;
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
                   Matrix.CreateTranslation(Position);
        }

        public void Render(Matrix _view, Matrix _projection) // Receives camera view matrix and camera projection matrix
        {

            Shader.Parameters["World"].SetValue(GetTransform());
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
            Shader.Parameters["Texture"].SetValue(Texture);
            Shader.Parameters["Tint"].SetValue(Selected);


            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                mesh.Draw();
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

        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            string mesh = _stream.ReadString();
            string texture = _stream.ReadString();
            string shader = _stream.ReadString();
            Position = HelpDeserialize.Vec3(_stream);
            Rotation = HelpDeserialize.Vec3(_stream);
            Scale = _stream.ReadSingle();
            Create(_content, mesh, texture, shader, Position, Scale);
        }





    }
}


