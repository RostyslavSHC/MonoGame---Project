using Editor.Engine.Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

// Groups this code inside the Editor project
namespace Editor.Engine
{
    internal class Level : ISerializable // Represents one game level
    {
        // Accessors
        public Camera GetCamera() { return m_camera; } // Returns level's camera

        // Members
        private List<Models> m_models = new(); // Stores all models in this level
        private Camera m_camera = new(new Vector3(0, 0, 300), 16.0f / 9.0f); // Creates level camera (had to change Vector3(0, 2, 2) to Vector3(0, 1, 1) and 16/9 to 16.0f/9.0f) ------------------



        public Level() // Runs when level is created
        {
        }

        public void LoadContent(ContentManager _content) // Loads level's files and assets
        {
            //Models teapot = new(_content.Load<Model>("Teapot"), _content.Load<Texture>("Metal"), Vector3.Zero, 1.0f); // Creates teapot model
            //teapot.SetShader(_content.Load<Effect>("MyShader")); // Gives teapot its shader
            Models teapot = new(_content, "Teapot", "Metal", "MyShader", Vector3.Zero, 1.0f);
            AddModel(teapot); // Adds teapot to the level
        }

        public void AddModel(Models _model) // Adds model to level
        {
            m_models.Add(_model); // Stores model in list
        }

        public void Render() // Draws level
        {
            foreach (Models m in m_models) // Goes through every model
            {
                m.Render(m_camera.View, m_camera.Projection); // Draws model using camera
            }
        }



        // Serialization
        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(m_models.Count);

            foreach (var model in m_models)
            {
                model.Serialize(_stream);
            }

            m_camera.Serialize(_stream);
        }

        // Deserialization
        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            int modelCount = _stream.ReadInt32();

            for (int count = 0; count < modelCount; count++)
            {
                Models m = new();
                m.Deserialize(_stream, _content);
                m_models.Add(m);
            }

            m_camera.Deserialize(_stream, _content);
        }










    }
}