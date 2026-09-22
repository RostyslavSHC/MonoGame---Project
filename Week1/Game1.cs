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
        private IndexBuffer m_indexBuffer; // Stores index data that will be sent to the GPU

        // Matrices used for rendering
        private Matrix m_world = Matrix.Identity; // A matrix that will be used to transform the model
        private Matrix m_view = Matrix.Identity; // A matrix that will be used to transform the camera
        private Matrix m_projection = Matrix.Identity; // A matrix that will be used to project the model onto the screen

        private float m_rotation = 0.0f;


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

            // Icosahedron world position
            m_world = Matrix.CreateTranslation(new Vector3(0.5f, 0.5f, 0.5f));

            // Camera
            m_view = Matrix.CreateLookAt(new Vector3(0, 0, 3),   // Camera position
                                         new Vector3(0, 0, 0),   // Camera target
                                             Vector3.Up);             // Camera up

            // Perspective
            m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), m_device.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            base.Initialize();
        }




        protected override void LoadContent()
        {
            /*
            VertexPositionColor[] vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(+0.5f, 0, 0), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Blue);

            m_vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            m_vertexBuffer.SetData<VertexPositionColor>(vertices);
            */

            CreateVertices();
            CreateIndices();
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
            GraphicsDevice.Indices = m_indexBuffer;
            #endregion ConfigureDevice


            #region Render
            foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                // GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 20);
            }

            base.Draw(gameTime);
            #endregion Render
        }






        // This method is called once per frame. It is where we put our game logic, like checking for input, updating game objects, etc.
        protected override void Update(GameTime gameTime)
        {
            m_rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_world = Matrix.CreateRotationY(m_rotation) * Matrix.CreateTranslation(new Vector3(0.5f, 0.5f, 0.5f));

            base.Update(gameTime);
        }

















        private void CreateVertices()
        {
            // A temporary array, with 12 items in it, because the icosahedron has 12 distinct vertices
            VertexPositionColor[] vertices = new VertexPositionColor[12];

            vertices[0] = new VertexPositionColor(new Vector3(-0.26286500f, 0.00000000f, 0.42532500f), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(0.26286500f, 0.00000000f, 0.42532500f), Color.Orange);
            vertices[2] = new VertexPositionColor(new Vector3(-0.26286500f, 0.00000000f, -0.42532500f), Color.Yellow);
            vertices[3] = new VertexPositionColor(new Vector3(0.26286500f, 0.00000000f, -0.42532500f), Color.Green);
            vertices[4] = new VertexPositionColor(new Vector3(0.00000000f, 0.42532500f, 0.26286500f), Color.Blue);
            vertices[5] = new VertexPositionColor(new Vector3(0.00000000f, 0.42532500f, -0.26286500f), Color.Indigo);
            vertices[6] = new VertexPositionColor(new Vector3(0.00000000f, -0.42532500f, 0.26286500f), Color.Purple);
            vertices[7] = new VertexPositionColor(new Vector3(0.00000000f, -0.42532500f, -0.26286500f), Color.White);
            vertices[8] = new VertexPositionColor(new Vector3(0.42532500f, 0.26286500f, 0.00000000f), Color.Cyan);
            vertices[9] = new VertexPositionColor(new Vector3(-0.42532500f, 0.26286500f, 0.00000000f), Color.Black);
            vertices[10] = new VertexPositionColor(new Vector3(0.42532500f, -0.26286500f, 0.00000000f), Color.DodgerBlue);
            vertices[11] = new VertexPositionColor(new Vector3(-0.42532500f, -0.26286500f, 0.00000000f), Color.Crimson);

            m_vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
            m_vertexBuffer.SetData<VertexPositionColor>(vertices);
        }









        private void CreateIndices()
        {
            short[] indices = new short[60];

            indices[0] = 0; indices[1] = 6; indices[2] = 1;
            indices[3] = 0; indices[4] = 11; indices[5] = 6;
            indices[6] = 1; indices[7] = 4; indices[8] = 0;
            indices[9] = 1; indices[10] = 8; indices[11] = 4;
            indices[12] = 1; indices[13] = 10; indices[14] = 8;
            indices[15] = 2; indices[16] = 5; indices[17] = 3;
            indices[18] = 2; indices[19] = 9; indices[20] = 5;
            indices[21] = 2; indices[22] = 11; indices[23] = 9;
            indices[24] = 3; indices[25] = 7; indices[26] = 2;
            indices[27] = 3; indices[28] = 10; indices[29] = 7;
            indices[30] = 4; indices[31] = 8; indices[32] = 5;
            indices[33] = 4; indices[34] = 9; indices[35] = 0;
            indices[36] = 5; indices[37] = 8; indices[38] = 3;
            indices[39] = 5; indices[40] = 9; indices[41] = 4;
            indices[42] = 6; indices[43] = 10; indices[44] = 1;
            indices[45] = 6; indices[46] = 11; indices[47] = 7;
            indices[48] = 7; indices[49] = 10; indices[50] = 6;
            indices[51] = 7; indices[52] = 11; indices[53] = 2;
            indices[54] = 8; indices[55] = 10; indices[56] = 3;
            indices[57] = 9; indices[58] = 11; indices[59] = 0;




            m_indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            m_indexBuffer.SetData(indices);
        }








    }
}
