using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Monogame
{
    // Main game class. It inherits from MonoGame's Game class, which gives us Initialize, Update, Draw, etc
    public class Game1 : Game
    {
        GraphicsDeviceManager m_device; // Variable that manages the graphics device and settings
        Model m_model; // The variable our imported FBX model will use

        private Matrix m_world = Matrix.Identity; // A matrix that will be used to transform the model
        private Matrix m_view = Matrix.Identity; // A matrix that will be used to transform the camera
        private Matrix m_projection = Matrix.Identity; // A matrix that will be used to project the model onto the screen

        KeyboardState m_oldState; // This stores what the keyboard looked like during the previous update/frame. That lets us detect that the player just pressed A, instead of running something continuously while A is held






        // Constructor. Runs when Game1 is first created
        public Game1()
        {
            m_device = new GraphicsDeviceManager(this); // Create the graphics manager for this game.     "this" means this current Game1 object
            m_device.PreferredBackBufferWidth = 600;
            m_device.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
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
                MultiplyVector();
                CalculateDistance();
                DotProduct();
                CrossProduct();



                MatrixOperations();
            }
            m_oldState = newState; // Update the previous state of the keyboard
            base.Update(gameTime); // Call MonoGame's original Update method. This is important to keep the game running smoothly
        }



        protected override void LoadContent()
        {
            m_model = Content.Load<Model>("Teapot");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Clear the screen to black
            m_world *= Matrix.CreateRotationY(0.1f); // Rotate the object every frame

            
            foreach (var mesh in m_model.Meshes) // Loop through all the meshes in the model
                {
                foreach (BasicEffect effect in mesh.Effects) // Loop through all the effects in the mesh
                {
                    effect.World = m_world; // Set the world matrix
                    effect.View = m_view; // Set the view matrix
                    effect.Projection = m_projection; // Set the projection matrix
                }
                mesh.Draw(); // Draw the mesh
            }

            base.Draw(gameTime); // Call MonoGame's original Draw method

        }


        protected override void Initialize()
        {
            // World Matrix - Where the object is in a 3D space
            m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0)); // Create a translation matrix to move the object to pos1
            // View Matrix - Where the camera is in a 3D space
            m_view = Matrix.CreateLookAt(new Vector3(0, 0, 2), new Vector3(0, 0, 0), Vector3.Up); // Create a view matrix to look at the object from above
            // Projection Matrix - How the 3D object is projected onto the 2D screen
            m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), m_device.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f); // Create a projection matrix to project the object onto the screen

            base.Initialize(); // That is important because MonoGame's base Initialize() calls LoadContent(), which is where you do: 
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

        private void MultiplyVector()
        {
            Vector3 pos = new Vector3(0, 0, 0); // Create a 3D vector with x=0, y=0, z=0
            Vector3 dest = new Vector3(1, 0, 0); // Create another 3D vector with x=1, y=0, z=0
            Vector3 dir = dest - pos; // Subtract two vectors to get a direction vector
            dir.Normalize(); // Normalize the direction vector to make it a unit vector (length of 1)
            pos += (dir * 0.1f); // Move the position 0.1 units in the direction of the direction vector

            Debug.WriteLine($"Pos: {pos}"); // Print the new position to the debug console
        }

        private void CalculateDistance()
        {
            Vector3 p1 = new Vector3(1, 1, 0); // Create  3D vector with x=1, y=1, z=0
            Vector3 p2 = new Vector3(2, 1, 2); // Create another 3D vector with x=2, y=1, z=2
            float distance = Vector3.Distance(p1, p2); // Calculate distance between two vectors
            Debug.WriteLine($"Distance between {p1} and {p2}: {distance}"); // Print distance to debug console
        }

        private void DotProduct()
        {
            Vector3 source = new Vector3(0, 0, 0); // Create a 3D vector with x=0, y=0, z=0
            Vector3 a = new Vector3(0, 2, 0); // Create another 3D vector with x=0, y=2, z=0
            Vector3 b = new Vector3(1, 1, 0); // Create another 3D vector with x=1, y=1, z=0
            Vector3 aVec = a - source; // Subtract two vectors to get a direction vector
            Vector3 bVec = b - source; // Subtract two vectors to get a direction vector
            aVec.Normalize(); // Normalize the direction vector to make it a unit vector (length of 1)
            bVec.Normalize(); // Normalize the direction vector to make it a unit vector (length of 1)
            float dot = Vector3.Dot(aVec, bVec); // Calculate the dot product of two vectors

            Debug.WriteLine($"Dot = {dot}"); // Print the dot product to the debug console
        }

        private void CrossProduct()
        {
            Vector3 source = new Vector3(0, 0, 0); // Create a 3D vector with x=0, y=0, z=0
            Vector3 a = new Vector3(0, 2, 0); // Create another 3D vector with x=0, y=2, z=0
            Vector3 b = new Vector3(1, 1, 0); // Create another 3D vector with x=1, y=1, z=0
            Vector3 aVec = a - source; // Subtract two vectors to get a direction vector
            Vector3 bVec = b - source; // Subtract two vectors to get a direction vector
            aVec.Normalize(); // Normalize the direction vector to make it a unit vector (length of 1)
            bVec.Normalize(); // Normalize the direction vector to make it a unit vector (length of 1)
            Vector3 cross = Vector3.Cross(aVec, bVec); // Calculate the cross product of two vectors
            Debug.WriteLine($"Cross = {cross}"); // Print the cross product to the debug console
        }














        // Matrices
        private void MatrixOperations()
        {
            Debug.WriteLine("\n\n\n\n\n");

            Matrix matrix = Matrix.Identity; // Create an identity matrix (no transformation)
            Debug.WriteLine(matrix); // Print the identity matrix to the debug console

            Matrix matrix1 = new Matrix(new Vector4(1, 1, 1, 1), new Vector4(2, 2, 2, 2), new Vector4(3, 3, 3, 3), new Vector4(4, 4, 4, 4)); // Create a matrix with 4 rows and 4 columns
            Matrix matrix2 = new Matrix(new Vector4(5, 5, 5, 5), new Vector4(6, 6, 6, 6), new Vector4(7, 7, 7, 7), new Vector4(8, 8, 8, 8)); // Create another matrix with 4 rows and 4 columns

            Debug.WriteLine($"Matrix 1 = {matrix1}"); // Print the first matrix to the debug console
            Debug.WriteLine($"Matrix 2 = {matrix2}"); // Print the second matrix to the debug console
            Debug.WriteLine($"Added = {matrix1 + matrix2}"); // Print the result of adding the two matrices to the debug console
            Debug.WriteLine($"Subtracted = {matrix1 - matrix2}"); // Print the result of subtracting the two matrices to the debug console
            Debug.WriteLine($"Multiplied = {matrix1 * matrix2}"); // Print the result of multiplying the two matrices to the debug console

            // Different results
            Debug.WriteLine("\n");
            Debug.WriteLine("Result of multiplying matrices depends on the order of multiplication");
            Debug.WriteLine($"Matrix 1 * Matrix 2 = {matrix1 * matrix2}");
            Debug.WriteLine($"Matrix 2 * Matrix 1 = {matrix2 * matrix1}");

            Debug.WriteLine("\n");
            Debug.WriteLine("Multiply by Identity matrix won't change the answer");
            Debug.WriteLine($"Identity * Matrix 2 = {Matrix.Identity * matrix2}");
            Debug.WriteLine($"Matrix 2 * Identity = {matrix2 * Matrix.Identity}");


            Debug.WriteLine("\n");
            Debug.WriteLine("Transformations");
            Vector3 pos1 = new Vector3(0, 0, 0); // Create a 3D vector with x=0, y=0, z=0
            Vector3 dest1 = new Vector3(1, 2, 3); // Create another 3D vector with x=1, y=2, z=3
            Matrix objectPos1 = Matrix.CreateTranslation(pos1); // Create a translation matrix to move the object to pos1
            Matrix objectPos2 = Matrix.CreateTranslation(dest1); // Create a translation matrix to move the object to dest1
            Debug.WriteLine($"ObjectPos1 = {objectPos1}"); // Print the first matrix to the debug console
            Debug.WriteLine($"ObjectPos2 = {objectPos2}"); // Print the second matrix to the debug console


            Debug.WriteLine("\n");
            Debug.WriteLine("Scaling");
            Vector3 scale = new Vector3(2, 2, 2); // Create a 3D vector with x=2, y=2, z=2
            Matrix objectScale = Matrix.CreateScale(scale); // Create a scaling matrix to scale the object by 2 in all directions
            Debug.WriteLine($"ObjectScale = {objectScale}"); // Print the scaling matrix to the debug console

            Debug.WriteLine("\n");
            Debug.WriteLine("Rotation");
            Vector3 rotate = new Vector3(0, 0.1f, 0); // Create a 3D vector with x=0, y=0.1, z=0
            Matrix objectRotate = Matrix.CreateRotationY(rotate.Y); // Create a rotation matrix to rotate the object around the y-axis by 0.1 radians
            Debug.WriteLine($"ObjectRotate = {objectRotate}"); // Print the rotation matrix to the debug console




    }









    }
}
