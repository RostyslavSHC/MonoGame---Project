using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Monogame
{
    // Main game class. It inherits from MonoGame's Game class, which gives us Initialize, Update, Draw, etc
    public class Game1 : Game
    {
        GraphicsDeviceManager m_device; // Variable that manages the graphics device and settings
        Model m_model; // The variable our imported FBX model will use

        private Matrix m_world = Matrix.Identity; // Matrix that will be used to transform the model
        private Matrix m_view = Matrix.Identity; // Matrix that will be used to transform the camera
        private Matrix m_projection = Matrix.Identity; // A matrix that will be used to project the model onto the screen
        private float m_rotation = 0f; // Variable that will be used to rotate the model


        // EXTRA - Text
        SpriteBatch m_spriteBatch; // SpriteBatch - Used to draw 2D things like text and images on the screen
        SpriteFont m_font; // SpriteFont - Stores the font used to draw text




        // Constructor. Runs when Game1 is first created
        public Game1()
        {
            m_device = new GraphicsDeviceManager(this); // Create the graphics manager for this game.     "this" means this current Game1 object
            m_device.PreferredBackBufferWidth = 600;
            m_device.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = true; // Make the mouse cursor visible inside the game window
        }



        // This method is called once per frame. It is where we put our game logic, like checking for input, updating game objects, etc
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // This method is called once per frame. It is where we put our game logic, like checking for input, updating game objects, etc
        protected override void LoadContent()
        {
            m_model = Content.Load<Model>("Teapot");

            // EXTRA - Text
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_font = Content.Load<SpriteFont>("Font");
        }





        // Draw method. This is where we draw our game objects
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Clear the screen to black

            m_rotation += 0.01f; // Increase the rotation angle every frame by 0.01

            // Variable that used to transform the object
            m_world =
                Matrix.CreateScale(2f) * // Scale the object
                Matrix.CreateRotationX(m_rotation) * // Rotate around X
                Matrix.CreateRotationY(m_rotation) * // Rotate around Y
                Matrix.CreateRotationZ(m_rotation) * // Rotate around Z
                Matrix.CreateTranslation(0, 0, 0); // Move the object

            foreach (var mesh in m_model.Meshes) // Loop through all the meshes in the model
            {
                foreach (BasicEffect effect in mesh.Effects) // Loop through all the effects in the mesh
                {
                    effect.World = m_world; // Set the world matrix
                    effect.View = m_view; // Set the view matrix
                    effect.Projection = m_projection; // Set the projection matrix
                }
                mesh.Draw(); // Draw the mesh
            }




            // EXTRA - Text
            m_spriteBatch.Begin();
            // Vector2(X, Y)
            m_spriteBatch.DrawString(m_font, $"Rotation: {m_rotation}", new Vector2(10, 10), Color.White);
            m_spriteBatch.DrawString(m_font, "Hello, Rogerio", new Vector2(10, 30), Color.Crimson);
            m_spriteBatch.DrawString(m_font, "I am Rostyslav", new Vector2(10, 50), Color.Cyan);
            m_spriteBatch.End();




            base.Draw(gameTime); // Call MonoGame's original Draw method
        }



        // Initialize method. This is where we initialize our game objects
        protected override void Initialize()
        {
            // World Matrix - Where the object is in a 3D space
            m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0)); // Create a translation matrix to move the object to pos1
            // View Matrix - Where the camera is in a 3D space
            m_view = Matrix.CreateLookAt(new Vector3(0, 0, 2), new Vector3(0, 0, 0), Vector3.Up); // Create a view matrix to look at the object from above
            // Projection Matrix - How the 3D object is projected onto the 2D screen
            m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), m_device.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f); // Create a projection matrix to project the object onto the screen

            base.Initialize(); // That is important because MonoGame's base Initialize() calls LoadContent(), which is where you do: 
        }






    }
}
