namespace Week_2___Matrices___Task_1___Matrix_Operations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ask user for matrix size
            Console.WriteLine("Enter number of rows, columns for matrices (R C)"); // Ask user how many rows and columns split by a space
            string[] input = Console.ReadLine().Split(' '); // Read user input and split it into an array of strings
            int rows = int.Parse(input[0]); // Parse the first element of the array as an integer and store it in the rows variable
            int columns = int.Parse(input[1]); // Parse the second element of the array as an integer and store it in the columns variable

            // Create matrix1 and matrix2
            Matrix matrix1 = new Matrix(rows, columns);
            Matrix matrix2 = new Matrix(rows, columns);



            // Ask user for values for matrix1 and matrix2
            Console.WriteLine("\nEnter values for Matrix 1: ");
            matrix1.matrixInput();

            Console.WriteLine("\nEnter values for Matrix 2: ");
            matrix2.matrixInput();



            // Print matrix1 and matrix2
            Console.WriteLine("\nMatrix 1: ");
            matrix1.PrintMatrix();

            Console.WriteLine("\nMatrix 2: ");




            // Add matrices
            Matrix additionMatrix = Matrix.AddMatrices(matrix1, matrix2);
            Console.WriteLine("\nAddition Matrix: ");
            additionMatrix.PrintMatrix();

            // Subtract matrices
            Matrix subtractionMatrix = Matrix.SubtractMatrices(matrix1, matrix2);
            Console.WriteLine("\nSubtraction Matrix: ");
            subtractionMatrix.PrintMatrix();

            // Multiply matrices with a small check to make sure they are the same size
            if (rows == columns)
            {
                Matrix multiplicationMatrix = Matrix.MultiplyMatrices(matrix1, matrix2);
                Console.WriteLine("\nMultiplication Matrix: ");
                multiplicationMatrix.PrintMatrix();
            }
            else
            {
                Console.WriteLine("\nMatrices must be the same size for multiplication. Rows and columns must be equal");
            }

        }
    }
}