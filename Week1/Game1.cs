using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Week1
{
    // Main game class. It inherits from MonoGame's Game class, which gives us Initialize, Update, Draw, etc
    public class Game1 : Game
    {
        GraphicsDeviceManager m_device; // Variable that manages the graphics device and settings
        KeyboardState m_oldState; // This stores what the keyboard looked like during the previous update/frame. That lets us detect that the player just pressed A, instead of running something continuously while A is held


        // Constructor. Runs when Game1 is first created
        public Game1()
        {
            m_device = new GraphicsDeviceManager(this); // Create the graphics manager for this game.     "this" means this current Game1 object.
            IsMouseVisible = true; // Make the mouse cursor visible inside the game window
            m_oldState = Keyboard.GetState(); // Get the current state of the keyboard (What keys are pressed right now?)
        }


        // This method is called once per frame. It is where we put our game logic, like checking for input, updating game objects, etc.
        protected override void Update(GameTime gameTime)
        {
            var newState = Keyboard.GetState(); // Get the current state of the keyboard (which keys are pressed or released)

            if (newState.IsKeyDown(Keys.A) && m_oldState.IsKeyUp(Keys.A)) // If the A key is pressed and it was not pressed before
            {
                AddVectors();
                SubtractVectors();
            }
            m_oldState = newState; // Update the previous state of the keyboard
            base.Update(gameTime); // Call MonoGame's original Update method. This is important to keep the game running smoothly
        }


        private void AddVectors()
        {
            Vector2 a = new Vector2(3, 5); // Create a 2D vector with x=3 and y=5
            Vector2 b = new Vector2(2, -1); // Create another 2D vector with x=2 and y=-1
            Vector2 c = a + b; // Add 2 vectors together

            Debug.WriteLine($"Vector a {a} + b {b} = {c}"); // Print the result to the debug console
        }

        private void SubtractVectors()
        {
            Vector2 a = new Vector2(3, 5); // Create a 2D vector with x=3 and y=5
            Vector2 b = new Vector2(2, -1); // Create another 2D vector with x=2 and y=-1
            Vector2 c = b - a; // Subtract 2 vectors
            Vector2 d = a - b; // Subtract 2 vectors

            Debug.WriteLine($"Vector c mag: {c.Length()} c dir: {c}"); // Print the magnitude of vector c to the debug console
            Debug.WriteLine($"Vector d mag: {d.Length()} d dir: {d}"); // Print the magnitude of vector d to the debug console
        }









    }
}
