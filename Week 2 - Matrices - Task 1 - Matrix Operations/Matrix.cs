using System;
using System.Collections.Generic;
using System.Text;

namespace Week_2___Matrices___Task_1___Matrix_Operations
{
    internal class Matrix
    {
        // Store all matrix numbers
        private double[,] matrix; // 2D array to hold the matrix values
        public int Rows { get; set; } // Number of rows in the matrix
        public int Columns { get; set; } // Number of columns in the matrix

        // Constructor
        public Matrix(int rows, int columns)
        {
            Rows = rows; // Set the number of rows
            Columns = columns; // Set the number of columns
            matrix = new double[rows, columns]; // Initialize the 2D array with the specified dimensions
        }


        // Ask user for matrix values
        public void matrixInput()
        {
            for (int row = 0; row < Rows; row++) // Loop through each row
            {
                for (int column = 0; column < Columns; column++) // Loop through each column
                {
                    Console.Write($"Enter value for row {row}, column {column}: "); // Prompt for user input
                    matrix[row, column] = double.Parse(Console.ReadLine()); // Read user input and store it in the matrix array
                }
            }
            
        }


        // Print the matrix
        public void PrintMatrix()
        {
            for (int row = 0; row < Rows; row++) // Loop through each row
            {
                for (int column = 0; column < Columns; column++) // Loop through each column
                {
                    Console.Write(matrix[row, column] + "\t"); // Print each value followed by a tab
                }
                Console.WriteLine(); // Move to the next line after each row
            }


        }



        // Add matrices
        public static Matrix AddMatrices(Matrix matrix1, Matrix matrix2)
        {
            Matrix AdditionMatrix = new Matrix(matrix1.Rows, matrix1.Columns); // Create a new matrix to hold the result of the addition

            for (int row = 0; row < matrix1.Rows; row++) // Loop through each row
            {
                for (int column = 0; column < matrix1.Columns; column++) // Loop through each column
                {
                    AdditionMatrix.matrix[row, column] = matrix1.matrix[row, column] + matrix2.matrix[row, column]; // Add corresponding values from both matrices
                }
            }
            return AdditionMatrix;
        }


        // Subtract matrices
        public static Matrix SubtractMatrices(Matrix matrix1, Matrix matrix2)
        {
            Matrix SubtractionMatrix = new Matrix(matrix1.Rows, matrix1.Columns); // Create a new matrix to hold the result of the subtraction

            for (int row = 0; row < matrix1.Rows; row++) // Loop through each row
            {
                for (int column = 0; column < matrix1.Columns; column++) // Loop through each column
                {
                    SubtractionMatrix.matrix[row, column] = matrix1.matrix[row, column] - matrix2.matrix[row, column]; // Subtract corresponding values from both matrices
                }
            }
            return SubtractionMatrix;
        }



        // Multiply matrices
        public static Matrix MultiplyMatrices(Matrix matrix1, Matrix matrix2)
        {
            Matrix MultiplicationMatrix = new Matrix(matrix1.Rows, matrix1.Columns); // Create a new matrix to hold the result of the multiplication

            for (int row = 0; row < matrix1.Rows; row++) // Loop through each row
            {
                for (int column = 0; column < matrix1.Columns; column++) // Loop through each column
                {
                    for (int i = 0; i < matrix1.Columns; i++)
                    {
                        MultiplicationMatrix.matrix[row, column] += matrix1.matrix[row, i] * matrix2.matrix[i, column]; // Multiply corresponding values from both matrices
                    }
                }
            }
            return MultiplicationMatrix;
        }














    }

}
