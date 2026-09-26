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
            base.Initialize(); // Runs MonoGame's normal setup
        }

        // Used to load game files and assets
        protected override void LoadContent()
        {

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

            base.Draw(gameTime); // Runs MonoGame's normal drawing
        }
    }
}
1