using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;

// Groups this code inside the Editor project
namespace Editor
{
    public class GameEditor : Game // Creates the MonoGame editor
    {
        private GraphicsDeviceManager m_graphics; // Manages the graphics device
        private FormEditor m_parent; // Stores the main editor window
        private Camera m_camera; // Stores the camera
        private Models m_teapot; // Stores the 3D model
        private Effect m_myShader; // Stores the shader
        private Texture m_metalTexture; // Stores the texture

        public GameEditor() // Runs when GameEditor is created
        {
            m_graphics = new GraphicsDeviceManager(this); // Creates the graphics manager
            Content.RootDirectory = "Content"; // Sets folder for game files
            IsMouseVisible = true; // Makes mouse cursor visible
        }

        // Creates GameEditor inside the editor form
        public GameEditor(FormEditor _parent) : this()
        {
            m_parent = _parent; // Saves parent editor window
            Form gameForm = Control.FromHandle(Window.Handle) as Form; // Gets MonoGame window as a Windows Form
            gameForm.TopLevel = false; // Makes it part of another window
            gameForm.Dock = DockStyle.Fill; // Makes it fill the available space
            gameForm.FormBorderStyle = FormBorderStyle.None; // Removes the window border
            m_parent.splitContainer.Panel1.Controls.Add(gameForm); // Adds the game window into Panel1
        }

        // Runs when the game starts
        protected override void Initialize()
        {
            m_camera = new Camera(new Vector3(0, 1, 1), m_graphics.GraphicsDevice.Viewport.AspectRatio); // Creates the camera and sets its position and aspect ratio
            RasterizerState state = new RasterizerState(); // Creates the rasterizer state
            state.CullMode = CullMode.None; // Disables culling
            GraphicsDevice.RasterizerState = state; // Sets the rasterizer state

            base.Initialize(); // Runs MonoGame's normal setup
        }

        // Used to load game files and assets
        protected override void LoadContent()
        {
            m_myShader = Content.Load<Effect>("MyShader"); // Loads the shader
            m_metalTexture = Content.Load<Texture>("Metal"); // Loads the texture

            m_teapot = new Models(Content.Load<Model>("Teapot"), m_metalTexture, Vector3.Zero, 1); // Creates the 3D model
            m_teapot.SetShader(m_myShader); // Sets the shader

        }

        // Runs every frame to update the game
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Runs MonoGame's normal update
        }

        // Runs every frame to draw the game
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clears the screen with a blue color

            m_teapot.Render(m_camera.View, m_camera.Projection); // Draws the 3D model
            base.Draw(gameTime); // Runs MonoGame's normal drawing
        }
    }
}