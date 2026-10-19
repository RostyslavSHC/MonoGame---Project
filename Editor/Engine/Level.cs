using Editor.Engine.Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Editor.Engine
{
    internal class Level : ISerializable // Represents one game level
    {
        // Accessors
        public Camera GetCamera() { return m_camera; }

        // Members
        private List<Models> m_models = new(); // Stores all models in this level
        private bool m_selectionChanged = true; // Tracks if the model selection changed
        private Camera m_camera = new(new Vector3(0, 0, -20), 16.0f / 9.0f); // Creates level camera at (0, 0, -20)



        public Level() // Constructor
        {
        }

        public void LoadContent(ContentManager _content) // Loads level's files and assets
        {
            // Creates teapot at (0, 0, 0) with scale 15 and adds it to level
            Models teapot = new(_content, "Teapot", "Metal", "MyShader", Vector3.Zero, 15.0f);
            AddModel(teapot);
        }

        public void AddModel(Models _model) // Adds model to level
        {
            m_models.Add(_model); // Stores model in list
        }


        public List<Models> GetSelectedModels()
        {
            List<Models> models = new List<Models>(); // Create an empty list

            foreach (var model in m_models) // Go through all models
            {
                if (model.Selected) models.Add(model); // Add selected models to the list
            }

            return models; // Return all selected models
        }

        // Check if selection changed, if so mark it as updated, if not return false
        public bool HasSelectionChanged() // Checks if the selection changed
        {
            return m_selectionChanged;
        }

        public void ClearSelectionChanged() // Marks the selection as updated
        {
            m_selectionChanged = false;
        }

        public void MarkSelectionChanged() // Marks selection as changed
        {
            m_selectionChanged = true; // PropertyGrid needs updating
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


            if (translate != Vector3.Zero) // Check if there is movement
            {
                bool modelTranslated = false; // Track if any model was moved

                foreach (Models model in m_models) // Go through all models
                {
                    if (model.Selected) // Check if the model is selected
                    {
                        modelTranslated = true; // Mark that a model was moved
                        model.Translate(translate / 1000, m_camera); // Move the selected model
                    }
                }

                if (!modelTranslated) // If no model was moved
                {
                    m_camera.Translate(translate * 0.001f); // Move the camera instead
                }
            }

        }






        private void HandleRotate(float _delta)
        {
            InputController ic = InputController.Instance; // Get input controller

            // Check if right mouse button is pressed without Alt
            if ((ic.IsButtonDown(MouseButtons.Right)) && (!ic.IsKeyDown(Keys.Menu)))
            {
                Vector2 dir = ic.MousePosition - ic.LastPosition; // Get mouse movement

                if (dir != Vector2.Zero) // Check if mouse moved
                {
                    Vector3 movement = new Vector3(dir.Y, dir.X, 0) * _delta; // Calculate rotation
                    bool modelRotated = false; // Track if a model was rotated

                    foreach (Models model in m_models) // Go through all models
                    {
                        if (model.Selected) // Check if model is selected
                        {
                            modelRotated = true; // Mark that a model was rotated
                            model.Rotation += movement; // Rotate selected model
                        }
                    }

                    if (!modelRotated) // If no model is selected
                    {
                        m_camera.Rotate(movement); // Rotate camera instead
                    }
                }
            }
        }


        private void HandleScale(float _delta)
        {
            InputController ic = InputController.Instance; // Get input controller

            // Check if right mouse button and Alt are pressed
            if ((ic.IsButtonDown(MouseButtons.Right)) &&
                (ic.IsKeyDown(Keys.Menu)))
            {
                float l = ic.MousePosition.X - ic.LastPosition.X; // Get horizontal mouse movement

                if (l != 0) // Check if mouse moved
                {
                    l *= _delta; // Adjust scaling using time between frames

                    foreach (Models model in m_models) // Go through all models
                    {
                        if (model.Selected) // Check if model is selected
                        {
                            model.Scale += l; // Increase or decrease model size
                        }
                    }
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
                    bool wasSelected = model.Selected; // Store previous selection
                    bool isSelected = false; // Track if the model is clicked

                    foreach (ModelMesh mesh in model.Mesh.Meshes) // Check every mesh
                    {
                        BoundingSphere s = mesh.BoundingSphere; // Get mesh bounding sphere
                        s = s.Transform(model.GetTransform()); // Transform sphere to world position

                        float? f = r.Intersects(s); // Check if mouse ray hits the model

                        if (f.HasValue) // If the model was clicked
                        {
                            isSelected = true; // Select model
                            break; // No need to check other meshes
                        }
                    }

                    model.Selected = isSelected; // Update model selection

                    if (wasSelected != isSelected) // Check if selection changed
                    {
                        m_selectionChanged = true; // PropertyGrid needs updating
                    }
                }
            }
        }















        public void Update(float _delta)
        {
            HandleTranslate(_delta); // Handle moving the camera
            HandleRotate(_delta); // Handle rotating the camera
            HandleScale(_delta); // Handle scaling the camera
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
            string s = string.Empty; // Start with empty text

            foreach (Models m in m_models) // Go through all models
            {
                if (m.Selected) // Check if the model is selected
                {
                    // Add selected model's position and rotation to the text
                    s += "\nModel: Pos: " + m.Position.ToString() +
                         "  Rot: " + m.Rotation.ToString();
                }
            }

            return m_camera.ToString() + s; // Show camera position and selected model information
        }











    }
}