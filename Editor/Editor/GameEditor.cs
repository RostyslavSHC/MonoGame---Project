using Editor.Editor;
using Editor.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Editor.Editor
{
    public class GameEditor : Game
    {
        // Properties
        internal Project Project { get; set; }
        private GraphicsDeviceManager m_graphics;
        private FormEditor m_parent;
        private SpriteBatch m_spriteBatch;
        private FontController m_fonts;

        RasterizerState m_rasterState = new RasterizerState(); // Controls how 3D surfaces are drawn
        DepthStencilState m_depthStencilState = new DepthStencilState(); // Controls which objects appear in front

        public GameEditor()
        {
            m_graphics = new GraphicsDeviceManager(this); // Creates graphics manager
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_rasterState = new RasterizerState();
            m_rasterState.CullMode = CullMode.None; // Draws both sides of 3D surfaces
            m_depthStencilState = new DepthStencilState();
            m_depthStencilState.DepthBufferEnable = true; // Enables depth testing
        }


        // Creates GameEditor inside the editor form
        public GameEditor(FormEditor _parent) : this()
        {
            m_parent = _parent; // Saves parent editor window
            Form gameForm = Control.FromHandle(Window.Handle) as Form;
            gameForm.TopLevel = false;
            gameForm.Dock = DockStyle.Fill;
            gameForm.FormBorderStyle = FormBorderStyle.None;
            m_parent.splitContainer.Panel1.Controls.Add(gameForm);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        // Used to load game files and assets
        protected override void LoadContent()
        {
            m_spriteBatch = new(GraphicsDevice);
            m_fonts = new();
            m_fonts.LoadContent(Content);
        }


        // Refreshes the PropertyGrid when a model property changes
        private void ModelPropertyChanged(object _sender, System.ComponentModel.PropertyChangedEventArgs _e)
        {
            if (_e.PropertyName == nameof(Models.Selected)) // Check if selection status changed
            {
                Project.CurrentLevel.MarkSelectionChanged(); // Mark selection for updating
            }

            m_parent.propertyGrid.Refresh(); // Refresh the PropertyGrid to show updated values
        }



        // Runs every frame to update the game
        protected override void Update(GameTime gameTime)
        {
            if (Project != null) // Check if a project exists
            {
                Project.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000); // Update the project using time between frames
                InputController.Instance.Clear();
                if (Project.CurrentLevel.HasSelectionChanged()) // Update PropertyGrid only when selection changes
                {
                    // Stop listening to previously selected models
                    foreach (Models model in m_parent.propertyGrid.SelectedObjects)
                    {
                        model.PropertyChanged -= ModelPropertyChanged;
                    }


                    var models = Project.CurrentLevel.GetSelectedModels();

                    if (models.Count == 0) // No models selected, clear PropertyGrid
                    {
                        m_parent.propertyGrid.SelectedObject = null;
                    }
                    else if (models.Count > 1) // Multiple models selected, show all
                    {
                        m_parent.propertyGrid.SelectedObjects = models.ToArray();
                    }
                    else // One model selected, show it
                    {
                        m_parent.propertyGrid.SelectedObject = models[0];
                    }

                    // Listen for changes to newly selected models
                    foreach (Models model in models)
                    {
                        model.PropertyChanged += ModelPropertyChanged;
                    }

                    Project.CurrentLevel.ClearSelectionChanged(); // Mark selection as updated
                }
            }

            base.Update(gameTime); // Run normal game update
        }


        // Runs every frame to draw the game
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen with a blue background

            if (Project != null)
            {
                GraphicsDevice.RasterizerState = m_rasterState;
                GraphicsDevice.DepthStencilState = m_depthStencilState;
                Project.Render();

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
            if (Project == null) return;
            Camera c = Project.CurrentLevel.GetCamera();
            c.Viewport = m_graphics.GraphicsDevice.Viewport;
            c.Update(c.Position, m_graphics.GraphicsDevice.Viewport.AspectRatio);
        }
    }
}
