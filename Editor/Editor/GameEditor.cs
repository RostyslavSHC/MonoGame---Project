using Editor.Editor;
using Editor.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// Groups this code inside the Editor project
namespace Editor.Editor
{
    public class GameEditor : Game // Creates the MonoGame editor
    {
        private Camera m_camera; // Stores the camera
        private Models m_teapot; // Stores the 3D model
        private Effect m_myShader; // Stores the shader
        private Texture m_metalTexture; // Stores the texture


        internal Project Project { get; set; } // Stores the project

        private GraphicsDeviceManager m_graphics; // Manages the graphics device
        private FormEditor m_parent; // Stores the main editor window

        private SpriteBatch m_spriteBatch; // Draws 2D elements
        private FontController m_fonts; // Handles fonts

        // private Level m_level; // Stores the level

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
            // m_camera = new Camera(new Vector3(0, 1, 1), m_graphics.GraphicsDevice.Viewport.AspectRatio); // Creates the camera and sets its position and aspect ratio
            // RasterizerState state = new RasterizerState(); // Creates the rasterizer state
            // state.CullMode = CullMode.None; // Disables culling
            // GraphicsDevice.RasterizerState = state; // Sets the rasterizer state

            base.Initialize(); // Runs MonoGame's normal setup
        }

        // Used to load game files and assets
        protected override void LoadContent()
        {
            /*
            m_level = new(); // Creates the level
            m_level.LoadContent(Content); // Loads level's files and assets
            AdjustAspectRatio(); // Adjusts the aspect ratio
            */

            /*
            m_myShader = Content.Load<Effect>("MyShader"); // Loads the shader
            m_metalTexture = Content.Load<Texture>("Metal"); // Loads the texture

            m_teapot = new Models(Content.Load<Model>("Teapot"), m_metalTexture, Vector3.Zero, 1); // Creates the 3D model
            m_teapot.SetShader(m_myShader); // Sets the shader
            */

            m_spriteBatch = new(GraphicsDevice); // Creates the sprite batch
            m_fonts = new(); // Creates the font controller
            m_fonts.LoadContent(Content); // Loads the fonts
        }

        // Runs every frame to update the game
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Runs MonoGame's normal update
        }

        // Runs every frame to draw the game
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen with a blue background
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (Project != null)
            {
                // Allow both sides of the 3D models to be visible
                RasterizerState state = new RasterizerState();
                state.CullMode = CullMode.None;
                GraphicsDevice.RasterizerState = state;

                Project.Render(); // Draw the project
                m_spriteBatch.Begin(); // Start drawing text
                m_fonts.Draw(m_spriteBatch, 20, InputController.Instance.ToString(), new Vector2(20, 20), Color.White); // Show currently pressed keyboard keys and mouse buttons
                m_spriteBatch.End(); // Finish drawing text
            }

            base.Draw(gameTime);
        }

        // Adjusts the aspect ratio of the camera
        public void AdjustAspectRatio()
        {
            if (Project == null) return; // If the project is not loaded, do nothing
            Camera c = Project.CurrentLevel.GetCamera(); // Gets the camera
            c.Update(c.Position, m_graphics.GraphicsDevice.Viewport.AspectRatio); // Updates the camera
        }
    }
}