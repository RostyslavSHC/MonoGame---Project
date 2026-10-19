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
        public Model Mesh { get; set; } // Stores 3D model

        [Browsable(false)]
        public Effect Shader { get; set; } // Stores shader used to draw model


        [Category("Transformation")]
        [Description("Position of the model in world space.")]
        public Vector3 Position // Gets or changes the model position
        {
            get => m_position;
            set
            {
                if (m_position != value) // Check if position changed
                {
                    m_position = value; // Save new position
                    OnPropertyChanged(nameof(Position)); // Notify PropertyGrid
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
                if (m_rotation != value) // Check if rotation changed
                {
                    m_rotation = value; // Save new rotation
                    OnPropertyChanged(nameof(Rotation)); // Notify PropertyGrid
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
                if (m_scale != value) // Check if scale changed
                {
                    m_scale = value; // Save new scale
                    OnPropertyChanged(nameof(Scale)); // Notify PropertyGrid
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
                if (m_selected != value) // Check if selection changed
                {
                    m_selected = value; // Save selection status
                    OnPropertyChanged(nameof(Selected)); // Notify PropertyGrid
                }
            }
        }



        // Texturing
        [Browsable(false)]
        public Texture Texture { get; set; } // Stores texture placed on model

        [Category("Appearance")]
        [Description("Diffuse texture of the model.")]
        [TypeConverter(typeof(DiffuseTextureConverter))]
        public string DiffuseTexture // Gets or changes the model texture
        {
            get => m_diffuseTexture;
            set
            {
                if (m_diffuseTexture != value) // Check if texture changed
                {
                    if (m_content != null) // Check if content manager exists
                    {
                        Texture newTexture = m_content.Load<Texture>(value); // Load new texture first
                        newTexture.Tag = value; // Store texture name for serialization
                        Texture = newTexture; // Apply new texture
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
        private bool m_selected = false; // Stores selection status
        private ContentManager m_content; // Stores content manager for loading textures
        private string m_diffuseTexture = "Metal"; // Stores selected texture name











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
            m_content = _content; // Store content manager for texture changes

            Mesh = _content.Load<Model>(_model);
            Mesh.Tag = _model;

            m_diffuseTexture = _texture; // Store initial texture name
            Texture = _content.Load<Texture>(_texture);
            Texture.Tag = _texture;



            Shader = _content.Load<Effect>(_effect);
            Shader.Tag = _effect;
            SetShader(Shader);
            Position = _position; // Set model position and notify if changed
            Scale = _scale; // Set model scale and notify if changed
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


