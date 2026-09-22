using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame
{
    // Main game class. It inherits from MonoGame's Game class, which gives us Initialize, Update, Draw, etc
    public class Game1 : Game
    {
        GraphicsDeviceManager m_device; // Variable that manages the graphics device and settings
        BasicEffect m_basicEffect; // Pre-made MonoGame shaders used for rendering
        VertexBuffer m_vertexBuffer; // Stores vertex data that will be sent to the GPU

        // Matrices used for rendering
        private Matrix m_world = Matrix.Identity; // A matrix that will be used to transform the model
        private Matrix m_view = Matrix.Identity; // A matrix that will be used to transform the camera
        private Matrix m_projection = Matrix.Identity; // A matrix that will be used to project the model onto the screen


        // Constructor. Runs when Game1 is first created
        public Game1()
        {
            m_device = new GraphicsDeviceManager(this); // Create the graphics manager for this game.     "this" means this current Game1 object
            IsMouseVisible = true; // Make the mouse cursor visible inside the game window
            Content.RootDirectory = "Content"; // Set the content directory to the "Content" folder
        }


        protected override void Initialize()
        {
            m_basicEffect = new BasicEffect(GraphicsDevice);

            m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            m_view = Matrix.CreateLookAt(new Vector3(0, 0, 3),   // Camera position
                                         new Vector3(0, 0, 0),   // Camera target
                                         Vector3.Up);            // Camera up

            m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), m_device.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            base.Initialize();
        }




        protected override void LoadContent()
        {
            // 8 vertices = 4 separate lines
            VertexPositionColor[] vertices = new VertexPositionColor[8];

            // Line 1
            vertices[0] = new VertexPositionColor(new Vector3(-0.8f, -0.6f, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(-0.7f, 0.8f, 0), Color.Green);

            // Line 2
            vertices[2] = new VertexPositionColor(new Vector3(-0.3f, -0.1f, 0), Color.Blue);
            vertices[3] = new VertexPositionColor(new Vector3(0.1f, 0.4f, 0), Color.Red);

            // Line 3
            vertices[4] = new VertexPositionColor(new Vector3(0.2f, -0.4f, 0), Color.Green);
            vertices[5] = new VertexPositionColor(new Vector3(0.6f, 0.1f, 0), Color.Blue);

            // Line 4
            vertices[6] = new VertexPositionColor(new Vector3(0.6f, -0.7f, 0), Color.Red);
            vertices[7] = new VertexPositionColor(new Vector3(0.9f, -0.2f, 0), Color.Green);

            m_vertexBuffer = new VertexBuffer(
                GraphicsDevice,
                typeof(VertexPositionColor),
                vertices.Length,
                BufferUsage.WriteOnly);

            m_vertexBuffer.SetData(vertices);
        }





        protected override void Draw(GameTime gameTime)
        {
            #region ConfigureBasicEffect
            m_basicEffect.World = m_world;
            m_basicEffect.View = m_view;
            m_basicEffect.Projection = m_projection;
            m_basicEffect.VertexColorEnabled = true;
            #endregion ConfigureBasicEffect


            #region ConfigureDevice
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetVertexBuffer(m_vertexBuffer);
            #endregion ConfigureDevice


            #region Render
            foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 4);
            }

            base.Draw(gameTime);
            #endregion Render
        }






        // This method is called once per frame. It is where we put our game logic, like checking for input, updating game objects, etc.
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Call MonoGame's original Update method. This is important to keep the game running smoothly
        }


      

    }
}
