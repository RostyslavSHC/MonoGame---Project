using Editor.Engine.Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

// Groups this code inside the Editor project
namespace Editor.Engine
{
    internal class Level : ISerializable // Represents one game level
    {
        // Accessors
        public Camera GetCamera() { return m_camera; } // Returns level's camera

        // Members
        private List<Models> m_models = new(); // Stores all models in this level
        private Camera m_camera = new(new Vector3(0, 1, 1), 16.0f / 9.0f); // Creates level camera (had to change Vector3(0, 2, 2) to Vector3(0, 1, 1) and 16/9 to 16.0f/9.0f) ------------------



        public Level() // Runs when level is created
        {
        }

        public void LoadContent(ContentManager _content) // Loads level's files and assets
        {
            //Models teapot = new(_content.Load<Model>("Teapot"), _content.Load<Texture>("Metal"), Vector3.Zero, 1.0f); // Creates teapot model
            //teapot.SetShader(_content.Load<Effect>("MyShader")); // Gives teapot its shader
            Models teapot = new(_content, "Teapot", "Metal", "MyShader", Vector3.Zero, 1.0f);
            AddModel(teapot); // Adds teapot to the level
            teapot = new(_content, "Teapot", "Metal", "MyShader", new Vector3(1, 0, 0), 1.0f);
            AddModel(teapot);
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








        public void HandleTranslate(float _delta)
        {
            InputController ic = InputController.Instance; // Gets keyboard and mouse input
            Vector3 translate = Vector3.Zero; // Stores camera movement

            if (ic.IsKeyDown(Keys.Left)) translate.X += -10; // Move left
            if (ic.IsKeyDown(Keys.Right)) translate.X += 10; // Move right

            if (ic.IsKeyDown(Keys.Menu)) // Check if Alt is pressed
            {
                if (ic.IsKeyDown(Keys.Up)) translate.Z += 1; // Zoom in
                if (ic.IsKeyDown(Keys.Down)) translate.Z += -1; // Zoom out
            }
            else
            {
                if (ic.IsKeyDown(Keys.Up)) translate.Y += 10; // Move up
                if (ic.IsKeyDown(Keys.Down)) translate.Y += -10; // Move down
            }

            if (ic.IsButtonDown(MouseButtons.Middle)) // Check if middle mouse button is pressed
            {
                Vector2 dir = ic.MousePosition - ic.LastPosition; // Calculate mouse movement
                translate.X = dir.X; // Move camera horizontally
                translate.Y = -dir.Y; // Move camera vertically
            }

            if (ic.GetWheel() != 0) // Check if mouse wheel was scrolled
            {
                translate.Z = ic.GetWheel() * 2; // Zoom using mouse wheel
            }

            if (translate != Vector3.Zero) // Only move camera if there is input
            {
                m_camera.Translate(translate * 0.001f); // Apply camera movement
            }
        }
        private void HandleRotate(float _delta)
        {
            InputController ic = InputController.Instance;

            if (ic.IsButtonDown(MouseButtons.Right)) // Check if right mouse button is pressed
            {
                Vector2 dir = ic.MousePosition - ic.LastPosition; // Calculate mouse movement

                if (dir != Vector2.Zero) // Only rotate if mouse moved
                {
                    Vector3 movement = new Vector3(dir.Y, dir.X, 0) * _delta; // Calculate rotation
                    m_camera.Rotate(movement); // Rotate camera
                }
            }
        }




        private void HandlePick()
        {
            InputController ic = InputController.Instance; // Get input controller

            if (ic.IsButtonDown(MouseButtons.Left)) // Check if left mouse button is pressed
            {
                Ray r = ic.GetPickRay(m_camera); // Create a ray from the mouse position

                foreach (Models model in m_models) // Check every model in the level
                {
                    model.Selected = false; // First unselect the model

                    foreach (ModelMesh mesh in model.Mesh.Meshes) // Check every mesh in the model
                    {
                        BoundingSphere s = mesh.BoundingSphere; // Get mesh bounding sphere
                        s = s.Transform(model.GetTransform()); // Move sphere to model's world position

                        float? f = r.Intersects(s); // Check if the ray hits the sphere
                        if (f.HasValue) // If hit happened
                        {
                            model.Selected = true; // Mark this model as selected
                        }
                    }
                }
            }
        }

        public void Update(float _delta)
        {
            HandleTranslate(_delta); // Handle moving the camera
            HandleRotate(_delta); // Handle rotating the camera
            HandlePick(); // Handle selecting objects with the mouse
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

        public override string ToString()
        {
            return m_camera.ToString();
        }










    }
}