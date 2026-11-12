using Editor.Engine.Interfaces;
using Editor.Engine.Lights;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Windows.Forms;
using Editor.Editor;

// Groups this code inside the Editor project
namespace Editor.Engine
{
    internal class Level : ISerializable // Represents one game level
    {
        // Accessors
        public Camera GetCamera() { return m_camera; } // Returns level's camera

        // Members
        private List<Models> m_models = new(); // Stores all models in this level
        private Camera m_camera = new(new Vector3(0, 400, 500), 16.0f / 9.0f);
        private Light m_light = new() { Position = new(0, 400, 500), Color = new(0.9f, 0.9f, 0.9f) };
        private Effect m_terrainEffect = null;
        private Terrain m_terrain = null;


        public Level() // Runs when level is created
        {
        }

        public void LoadContent(GameEditor _game)
        {
            m_terrain = new(_game.DefaultEffect, _game.DefaultHeightMap,
                            _game.DefaultGrass, 200, _game.GraphicsDevice);
        }

        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }

        public List<ISelectable> GetSelectedModels()
        {
            List<ISelectable> models = new();
            foreach (var model in m_models)
            {
                if (model.Selected) models.Add(model);
            }
            if (m_terrain != null)
            {
                if (m_terrain.Selected) models.Add(m_terrain);
            }

            return models;
        }








        public void Render()
        {
            Renderer r = Renderer.Instance;
            r.Camera = m_camera;
            r.Light = m_light;
            foreach (Models m in m_models)
            {
                r.Render(m);
            }
            if (m_terrain != null)
            {
                r.Render(m_terrain);
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




        internal ISelectable HandlePick(bool _select = true)
        {
            float? f;
            Matrix transform = Matrix.Identity;
            InputController ic = InputController.Instance;
            if (ic.IsButtonDown(MouseButtons.Left))
            {
                Ray r = HelpMath.GetPickRay(ic.MousePosition, m_camera);
                // Check models
                foreach (Models model in m_models)
                {
                    if (_select) model.Selected = false;
                    transform = model.GetTransform();
                    foreach (ModelMesh mesh in model.Mesh.Meshes)
                    {
                        BoundingSphere s = mesh.BoundingSphere;
                        s.Transform(ref transform, out s);
                        f = r.Intersects(s);
                        if (f.HasValue)
                        {
                            f = HelpMath.PickTriangle(in mesh, ref r, ref transform);
                            if (f.HasValue)
                            {
                                if (!_select) return model;
                                model.Selected = true;
                            }
                        }
                    }
                }

                // Check terrain
                if (m_terrain != null)
                {
                    transform = Matrix.Identity;
                    f = HelpMath.PickTriangle(in m_terrain, ref r, ref transform);
                    m_terrain.Selected = false;
                    if (f.HasValue)
                    {
                        if (!_select) return m_terrain;
                        m_terrain.Selected = true;
                    }
                }
            }

            return null;
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
        public void Deserialize(BinaryReader _stream, GameEditor _game)
        {
            int modelCount = _stream.ReadInt32();

            for (int count = 0; count < modelCount; count++)
            {
                Models m = new();
                m.Deserialize(_stream, _game);
                m_models.Add(m);
            }

            m_camera.Deserialize(_stream, _game);
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