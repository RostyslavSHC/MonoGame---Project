using Editor.Engine.Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;

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
        private Random m_random = new Random(); // NEW ----- Random number generator for level


        public Level() // Runs when level is created
        {
        }

        // This is where by default the game automatically creates models and add it to level. But now we will do it from menu so we create emoty screen
        public void LoadContent(ContentManager _content) // Loads level's files and assets
        {
            //Models teapot = new(_content.Load<Model>("Teapot"), _content.Load<Texture>("Metal"), Vector3.Zero, 1.0f); // Creates teapot model
            //teapot.SetShader(_content.Load<Effect>("MyShader")); // Gives teapot its shader
            //Models teapot = new(_content, "Teapot", "Metal", "MyShader", Vector3.Zero, 1.0f);
            //AddModel(teapot); // Adds teapot to the level
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





        // PLANET - Adding a planet to the level, but only if there are less than 5 planets already
        public void AddPlanet(ContentManager _content)
        {
            int planetCount = 0; // Counter of how many planets are already in the level
            foreach (Models model in m_models)
            {
                if (model.Name == "World")
                {
                    planetCount++;
                }
            }

            // Maximum 5 planets allowed
            if (planetCount >= 5)
            {
                return; // Do not add more planets
            }

            // Random position generation for new planet
            float x = m_random.Next(-150, 151);
            float y = m_random.Next(-90, 91);
            // I don't do anything for z because it should be 0

            float rotationSpeed = RandomFloat(0.02f, 0.03f); // Random rotation speed between 0.02 and 0.03
            float orbitSpeed = RandomFloat(0.001f, 0.002f); // Random orbit speed between 0.001 and 0.002

            Models planet = new(_content, "World", "WorldDiffuse", "MyShader", new Vector3(x, y, 0), 0.75f);

            planet.RotationSpeed = rotationSpeed;
            planet.OrbitSpeed = orbitSpeed;
            AddModel(planet);
        }

        // Helper method to generate a random float between min and max for Planet rotation and orbit speeds
        private float RandomFloat(float min, float max)
        {
            return (float)(m_random.NextDouble() * (max - min) + min);
        }


        // MOON - Adding a moon to the level, but only if there are less than 10 moons already
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

            // Add one mom for every existing planet
            foreach(Models planet in planets)
            {
                float scale = RandomFloat(0.2f, 0.4f); // Random scale for the moon
                float rotationSpeed = RandomFloat(0.005f, 0.01f); // Random rotation speed for the moon
                float orbitSpeed = RandomFloat(0.01f, 0.02f); // Random orbit speed for the moon

                Models moon = new(content, "Moon", "MoonDiffuse", "MyShader", planet.Position + new Vector3(planet.Position.X + 20, planet.Position.Y, planet.Position.Z), scale);
                moon.RotationSpeed = rotationSpeed;
                moon.OrbitSpeed = orbitSpeed;
                moon.Parent = planet; // Set the parent of the moon to the planet

                AddModel(moon); // Add the moon to the level
            }



        }











    }
}