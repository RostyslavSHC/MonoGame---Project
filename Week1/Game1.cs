using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Week1
{
    // Main game class. It inherits from MonoGame's Game class, which gives us Initialize, Update, Draw, etc
    public class Game1 : Game
    {
        GraphicsDeviceManager m_device; // Variable that manages the graphics device and settings
        Model m_model; // Variable that will store our loaded 3D model


        // Constructor. Runs when Game1 is first created
        public Game1()
        {
            m_device = new GraphicsDeviceManager(this); // Create the graphics manager for this game.     "this" means this current Game1 object.


            m_device.PreferredBackBufferWidth = 600; // Set the width of the game window to 600 pixels
            m_device.PreferredBackBufferHeight = 600; // Set the height of the game window to 600 pixels

            Content.RootDirectory = "Content"; // Tell MonoGame where our game assets are stored. For example: models, textures, sounds, fonts, etc
            IsMouseVisible = true; // Make the mouse cursor visible inside the game window

        }


        // Called once when the game starts. Usually used to set up variables, game objects, positions, etc
        protected override void Initialize()
        {
            // TODO means we can add our own initialization code here later.

            // Call MonoGame's original Initialize method.
            // This makes sure MonoGame also does its required setup.
            base.Initialize();
        }


        // Called once after Initialize().
        // Used to load game assets from the Content folder.
        protected override void LoadContent()
        {
            // Load the Teapot model from the Content folder.
            // We do NOT include the file extension here.
            //
            // For example:
            // Content/Teapot.fbx
            //
            // MonoGame processes it and loads it as a Model object.
            m_model = Content.Load<Model>("Teapot");
        }


        // Called every frame.
        // Used for game logic, movement, keyboard input, physics, etc.
        protected override void Update(GameTime gameTime)
        {
            // Get the current controller state for Player One.
            // Check if the Back button is pressed.
            //
            // OR
            //
            // Get the current keyboard state and check if Escape is pressed.
            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)
            )
            {
                // Close the game.
                Exit();
            }


            // TODO: Add your game logic here later.
            // For example:
            // movement
            // rotation
            // input
            // animations
            // collisions


            // Call MonoGame's original Update method.
            base.Update(gameTime);
        }


        // Called every frame after Update().
        // Used to draw/render everything on the screen.
        protected override void Draw(GameTime gameTime)
        {
            // Clear the previous frame.
            // Fill the whole screen with CornflowerBlue.
            //
            // Without Clear(), old frames could remain on the screen.
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // A Model can contain multiple Meshes.
            // A mesh is basically one drawable piece of the 3D model.
            //
            // Loop through every mesh inside our Teapot model.
            foreach (var mesh in m_model.Meshes)
            {
                // Draw the current mesh to the screen.
                mesh.Draw();
            }


            // Call MonoGame's original Draw method.
            base.Draw(gameTime);
        }
    }
}
