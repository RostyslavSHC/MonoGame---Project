using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

// Groups this code inside the Editor project
namespace Editor.Engine
{
    internal class Level : ISerializable
    {
        // Accessors
        public Camera GetCamera() { return m_camera; }

        // Members
        private List<Models> m_models = new(); // Stores all models in this level
        private Camera m_camera = new(new Vector3(0, 0, 300), 16.0f / 9.0f); // Creates camera at position (0, 0, 300) and aspect ratio 16:9. Camera is 300 units away from the center of the level, looking at the center (x=0, y=0)
        private Random m_random = new Random(); // Random number generator

        public Level()
        {
        }

        public void LoadContent(ContentManager _content)
        {
        }

        // Adds a model to the level by adding it to the list
        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }

        // Draws all models
        public void Render()
        {
            foreach (Models m in m_models)
            {
                m.Render(m_camera.View, m_camera.Projection);
            }
        }


        // Serialization
        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(m_models.Count);

            foreach (var model in m_models)
            {
                model.Serialize(_stream);

  
                int parentIndex = -1; // -1 - Model has no parent

                // If the model has a parent, find its index in the list and save that parent number
                if (model.Parent != null)
                {
                    parentIndex = m_models.IndexOf(model.Parent);
                }
                _stream.Write(parentIndex);
            }

            m_camera.Serialize(_stream);

        }

        // Deserialization
        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            int modelCount = _stream.ReadInt32();

            List<int> parentIndices = new List<int>(); // List to store parent indices

            // 1. Load all models
            for (int count = 0; count < modelCount; count++)
            {
                Models m = new();
                m.Deserialize(_stream, _content);
                m_models.Add(m);

                // Read which model is it's parent
                int parentIndex = _stream.ReadInt32();
                parentIndices.Add(parentIndex);
            }

            // 2. Set parents, after all models have been loaded
            for (int i = 0; i < modelCount; i++)
            {
                if (parentIndices[i] != -1) // Check if the model has a parent
                {
                    m_models[i].Parent = m_models[parentIndices[i]]; // Reconnect the Moon to the correct Planet
                    // Example:
                    // Before saving: Moon belongs to Planet #1
                    // So we save: Moon's parent number = 1
                    // When loading, we first create: #0 Sun, then #1 Planet, then #2 Moon
                        // m_models[i] - Model we are currently looking at. Example: if i = 2 : m_models[2]
                        // parentIndices[i] - Saved parent numer for that model. Example: parentIndices[2] = 1 : Model #2 has parent #1
                        // m_models[parentIndices[i]] - becomes m_models[1], which is the Planet that the Moon belongs to
                        // To sum up: Moon.Parent = Planet : Means that the Moon is now connected to the correct Planet, and will move with it when the Planet moves
                }
            }

            m_camera.Deserialize(_stream, _content);
        }









        // SUN - Adding a sun to the level, but only if it doesn't already exist
        public void AddSun(ContentManager _content)
        {
            // Check if the sun already exists in the level
            foreach(Models model in m_models)
            {
                if (model.Name == "Sun")
                {
                    return; // Sun already exists, do not add another
                }
            }
            Models sun = new(_content, "Sun", "SunDiffuse", "MyShader", Vector3.Zero, 2.0f);
            AddModel(sun);
        }


        // PLANET - Adding a planet to level, but only if there are less than 5 planets already
        public void AddPlanet(ContentManager _content)
        {
            int planetCount = 0;
            foreach (Models model in m_models)
            {
                // Count the number of planets
                if (model.Name == "World")
                {
                    planetCount++;
                }
            }

            // Maximum 5 planets allowed
            if (planetCount >= 5)
            {
                return;
            }

            // Random position generation for new planet
            float x = m_random.Next(-150, 151);
            float y = m_random.Next(-90, 91);
            // I don't do anything for z because it should be 0

            // Random rotation and orbit speeds between the specified values
            float rotationSpeed = RandomFloat(0.02f, 0.03f);
            float orbitSpeed = RandomFloat(0.001f, 0.002f);

            Models planet = new(_content, "World", "WorldDiffuse", "MyShader", new Vector3(x, y, 0), 0.75f); // Creates a new planet

            planet.RotationSpeed = rotationSpeed;
            planet.OrbitSpeed = orbitSpeed;
            AddModel(planet);
        }

        // Random number generator - Generates random float between min and max for planets' rotation and orbit speeds
        private float RandomFloat(float min, float max)
        {
            return (float)(m_random.NextDouble() * (max - min) + min);
            // 1. max - min = Size of the range
            // 2. NextDouble() * (max - min) = Random number between 0 and the size of the range
            // 3. min + Random number = Random number between min and max
        }


        // MOON - Adds one moon for every existing planet
        public void AddMoon(ContentManager content)
        {
            // List to store all existing planets in the level
            List<Models> planets = new List<Models>();

            // Find all planets in the level
            foreach (Models model in m_models)
            {
                if (model.Name == "World")
                {
                    planets.Add(model);
                }
            }

            // Add 1 mon for every existing planet
            foreach(Models planet in planets)
            {
                // Random scale, rotation, and orbit speeds generation
                float scale = RandomFloat(0.2f, 0.4f);
                float rotationSpeed = RandomFloat(0.005f, 0.01f);
                float orbitSpeed = RandomFloat(0.01f, 0.02f);

                Models moon = new(content, "Moon", "MoonDiffuse", "MyShader", planet.Position + new Vector3(20, 0, 0), scale);

                //Set rotation, orbit speeds, and parent of the moon
                moon.RotationSpeed = rotationSpeed;
                moon.OrbitSpeed = orbitSpeed;
                moon.Parent = planet;

                AddModel(moon); // Add the moon to the level
            }



        }











    }
}