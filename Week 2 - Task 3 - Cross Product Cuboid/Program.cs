using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Monogame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ask user for Point 1 position
            Console.WriteLine("Enter point 1 position (X Y Z):");
            string[] input1 = Console.ReadLine().Split(' ');
            float p1X = float.Parse(input1[0]);
            float p1Y = float.Parse(input1[1]);
            float p1Z = float.Parse(input1[2]);

            Vector3 p1 = new Vector3(p1X, p1Y, p1Z); // Create a 3D vector with the source position



            // Ask user for Point 2 position
            Console.WriteLine("Enter point 2 position (X Y Z):");
            string[] input2 = Console.ReadLine().Split(' ');
            float p2X = float.Parse(input2[0]);
            float p2Y = float.Parse(input2[1]);
            float p2Z = float.Parse(input2[2]);

            Vector3 p2 = new Vector3(p2X, p2Y, p2Z); // Create a 3D vector with the destination position


            // Ask user for Point 3 position
            Console.WriteLine("Enter point 3 position (X Y Z):");
            string[] input3 = Console.ReadLine().Split(' ');
            float p3X = float.Parse(input3[0]);
            float p3Y = float.Parse(input3[1]);
            float p3Z = float.Parse(input3[2]);

            Vector3 p3 = new Vector3(p3X, p3Y, p3Z); // Create a 3D vector with the destination position

            // Ask user for cuboid depth
            Console.WriteLine("Enter cuboid depth:");
            float depthInput = float.Parse(Console.ReadLine()); // Read the depth input from the user




            // Find the Point 4 position
            Vector3 p4 = p2 + p3 - p1; // Calculate the position of point 4 using vector addition and subtraction

            // Find 2 directions of the triangle
            Vector3 direction1 = p2 - p1; // Calculate the direction from point 1 to point 2
            Vector3 direction2 = p3 - p1; // Calculate the direction from point 1 to point 3

            Vector3 depthDirection = Vector3.Cross(direction1, direction2); // Calculate the depth direction using the cross product of the two directions
            depthDirection.Normalize(); // Normalize the depth direction to make it a unit vector (length of 1)
            float depth = depthInput; // Set the depth of the cuboid

            Vector3 offset = depthDirection * depth; // Calculate the offset vector by multiplying the depth direction by the depth

            // Create other 4 points of the cuboid by adding the offset to the original 4 points
            Vector3 p5 = p1 + offset; // Calculate the position of point 5 by adding the offset to point 1
            Vector3 p6 = p2 + offset; // Calculate the position of point 6 by adding the offset to point 2
            Vector3 p7 = p3 + offset; // Calculate the position of point 7 by adding the offset to point 3
            Vector3 p8 = p4 + offset; // Calculate the position of point 8 by adding the offset to point 4




            // Print everything
            Console.WriteLine("Cuboid Points");
            Console.WriteLine($"Point 1: {p1}");
            Console.WriteLine($"Point 2: {p2}");
            Console.WriteLine($"Point 3: {p3}");
            Console.WriteLine($"Point 4: {p4}");
            Console.WriteLine($"Point 5: {p5}");
            Console.WriteLine($"Point 6: {p6}");
            Console.WriteLine($"Point 7: {p7}");
            Console.WriteLine($"Point 8: {p8}");
        }
    }

}