using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Week1
{
    internal class Monogame
    {
        static void Main(string[] args)
        {
            // Ask user for Object 1 position
            Console.WriteLine("Enter object 1 position (X Y Z):");
            string[] input1 = Console.ReadLine().Split(' ');
            float p1X = float.Parse(input1[0]);
            float p1Y = float.Parse(input1[1]);
            float p1Z = float.Parse(input1[2]);

            Vector3 p1 = new Vector3(p1X, p1Y, p1Z); // Create a 3D vector with the source position



            // Ask user for Object 2 position
            Console.WriteLine("Enter object 2 position (X Y Z):");
            string[] input2 = Console.ReadLine().Split(' ');
            float p2X = float.Parse(input2[0]);
            float p2Y = float.Parse(input2[1]);
            float p2Z = float.Parse(input2[2]);

            Vector3 p2 = new Vector3(p2X, p2Y, p2Z); // Create a 3D vector with the destination position


            // Ask user for Object 3 position
            Console.WriteLine("Enter object 3 position (X Y Z):");
            string[] input3 = Console.ReadLine().Split(' ');
            float p3X = float.Parse(input3[0]);
            float p3Y = float.Parse(input3[1]);
            float p3Z = float.Parse(input3[2]);

            Vector3 p3 = new Vector3(p3X, p3Y, p3Z); // Create a 3D vector with the destination position





            // Find lengths of the sides of the triangle formed by the three points
            float sideA = Vector3.Distance(p1, p2); // Distance between p1 and p2
            float sideB = Vector3.Distance(p2, p3); // Distance between p2 and p3
            float sideC = Vector3.Distance(p3, p1); // Distance between p3 and p1

            float perimeter = sideA + sideB + sideC; // Calculate the perimeter of the triangle
            float semiPerimeter = perimeter / 2; // Calculate the semi-perimeter of the triangle

            float area = (float)Math.Sqrt(semiPerimeter * (semiPerimeter - sideA) * (semiPerimeter - sideB) * (semiPerimeter - sideC)); // Calculate the area of the triangle using Heron's formula

            Console.WriteLine($"Perimeter of the triangle: {perimeter}"); // Print the perimeter of the triangle
            Console.WriteLine($"Area of the triangle: {area}"); // Print the area of the triangle



        }
    }

}