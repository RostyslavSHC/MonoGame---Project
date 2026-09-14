using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Week1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ask user for source position
            Console.WriteLine("Enter source position (X Y Z):");
            string[] source = Console.ReadLine().Split(' ');
            float sourceX = float.Parse(source[0]);
            float sourceY = float.Parse(source[1]);
            float sourceZ = float.Parse(source[2]);

            Vector3 sourcePosition = new Vector3(sourceX, sourceY, sourceZ); // Create a 3D vector with the source position



            // Ask user for destination position
            Console.WriteLine("Enter destination position (X Y Z):");
            string[] destination = Console.ReadLine().Split(' ');
            float destinationX = float.Parse(destination[0]);
            float destinationY = float.Parse(destination[1]);
            float destinationZ = float.Parse(destination[2]);

            Vector3 destinationPosition = new Vector3(destinationX, destinationY, destinationZ); // Create a 3D vector with the destination position


            // Ask user how far the person can move in one step
            Console.WriteLine("Enter step distance:");
            float stepDistance = float.Parse(Console.ReadLine());

            
            float distance = Vector3.Distance(sourcePosition, destinationPosition); // Calculate the distance between the source and destination positions

            float steps = distance / stepDistance; // Calculate the number of steps needed


            // Display the result
            Console.WriteLine($"Distance between source and destination: {distance}");
            Console.WriteLine($"Number of steps needed: {steps}"); // Round up to the nearest whole number
        }
    }

}