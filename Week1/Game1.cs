using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame
{
    // Main game class. It inherits from MonoGame's Game class, which gives us Initialize, Update, Draw, etc
    public class Game1 : Game
    {
        private GraphicsDeviceManager m_device; // Display device
        private VertexBuffer m_vertexBuffer; // Stores vertex data that will be sent to the GPU
        private Effect m_myShader; // Our custom shader

        private SpriteBatch m_spriteBatch; // 2D sprite rendering - used to render text
        private SpriteFont m_arial12; // 2D sprite rendering - used to render text

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


        // Update - Used to update game state and it is called once per frame
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Call MonoGame's original Update method. This is important to keep the game running smoothly
        }



        // Initialize - Used to load content and it is called once when the game starts
        protected override void Initialize()
        {
            // Create 3 variables to hold the graphics device, sprite batch, and sprite font
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_arial12 = Content.Load<SpriteFont>("Arial12");
            m_myShader = Content.Load<Effect>("MyShader");

            // Set up matrices
            m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            m_view = Matrix.CreateLookAt(new Vector3(0, 0, 3),   // Camera position
                                         new Vector3(0, 0, 0),   // Camera target
                                         Vector3.Up);            // Camera up

            m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), m_device.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            base.Initialize();
        }



        // LoadContent - Used to load content and it is called once when the game starts
        protected override void LoadContent()
        {
            VertexPositionColor[] vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(+0.5f, 0, 0), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Blue);

            m_vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            m_vertexBuffer.SetData<VertexPositionColor>(vertices);
        }





        protected override void Draw(GameTime gameTime)
        {
            // Set up the shader
            #region ConfigureMyEffect
            m_myShader.Parameters["WorldViewProjection"].SetValue(m_world * m_view * m_projection);
            #endregion ConfigureMyEffect

            // Set up the graphics device / Color
            #region ConfigureDevice
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetVertexBuffer(m_vertexBuffer);
            #endregion ConfigureDevice

            // Font / Text
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(m_arial12, "Test String", new Vector2(10, 10), Color.Orange);
            m_spriteBatch.DrawString(m_arial12, "Test String", new Vector2(50, 50), Color.Orange, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0.5f);
            m_spriteBatch.DrawString(m_arial12, "Test String", new Vector2(100, 100), Color.Orange, 0, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 0.5f);

            string output = "Test String";
            Vector2 fontPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Vector2 origin = m_arial12.MeasureString(output) / 2;
            m_spriteBatch.DrawString(m_arial12, output, fontPos, Color.Orange, 0, origin, 1, SpriteEffects.None, 0.5f);
            m_spriteBatch.End();

            // Render / Draw
            #region Render
            foreach (EffectPass pass in m_myShader.Techniques["BasicColorDrawing"].Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            base.Draw(gameTime);
            #endregion Render
        }






 














    }
}
