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

    public class GameEditor : Game
    {
        internal Project Project { get; set; }

        private GraphicsDeviceManager m_graphics; // Manages graphics
        private FormEditor m_parent; // Stores the editor window
        private SpriteBatch m_spriteBatch; // Draws text on screen
        private FontController m_fonts; // Manages fonts

        RasterizerState m_rasterState = new RasterizerState(); // Controls how 3D surfaces are drawn
        DepthStencilState m_depthStencilState = new DepthStencilState(); // Controls which objects appear in front

        public GameEditor()
        {
            m_graphics = new GraphicsDeviceManager(this); // Creates graphics manager
            Content.RootDirectory = "Content"; // Sets content folder
            IsMouseVisible = true; // Shows mouse cursor

            m_rasterState = new RasterizerState(); // Creates rasterizer settings
            m_rasterState.CullMode = CullMode.None; // Draws both sides of 3D surfaces
            m_depthStencilState = new DepthStencilState(); // Creates depth settings
            m_depthStencilState.DepthBufferEnable = true; // Enables depth testing
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
            if (Project != null) // Check if a project exists
            {
                Project.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000); // Update the project using time between frames
                InputController.Instance.Clear(); // Reset mouse input for the next frame
                var models = Project.CurrentLevel.GetSelectedModels(); // Get all selected models

                if (models.Count == 0) // No models selected
                {
                    m_parent.propertyGrid.SelectedObject = null; // Clear property grid
                }
                else if (models.Count > 1) // Multiple models selected
                {
                    m_parent.propertyGrid.SelectedObjects = models.ToArray(); // Show multiple models
                }
                else // One model selected
                {
                    m_parent.propertyGrid.SelectedObject = models[0]; // Show selected model
                }
            }

            base.Update(gameTime); // Run normal game update
        }


        // Runs every frame to draw the game
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen with a blue background
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (Project != null) // Check if a project exists
            {
                GraphicsDevice.RasterizerState = m_rasterState; // Set how 3D surfaces are drawn
                GraphicsDevice.DepthStencilState = m_depthStencilState; // Enable depth testing
                Project.Render(); // Draw the project

                m_spriteBatch.Begin(); // Start drawing text
                m_fonts.Draw(m_spriteBatch, 20, InputController.Instance.ToString(), new Vector2(20, 20), Color.White); // Show pressed keys and buttons
                m_fonts.Draw(m_spriteBatch, 16, Project.CurrentLevel.ToString(), new Vector2(20, 80), Color.Yellow); // Show level information
                m_spriteBatch.End(); // Finish drawing text
            }


            base.Draw(gameTime);
        }

        // Adjusts the aspect ratio of the camera
        public void AdjustAspectRatio()
        {
            if (Project == null) return; // If the project is not loaded, do nothing
            Camera c = Project.CurrentLevel.GetCamera(); // Gets the camera
            c.Viewport = m_graphics.GraphicsDevice.Viewport; // Sets the viewport
            c.Update(c.Position, m_graphics.GraphicsDevice.Viewport.AspectRatio); // Updates the camera
        }
    }
}
