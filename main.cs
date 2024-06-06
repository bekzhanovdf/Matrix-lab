using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complex
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateDirectories();
            Matrix[] matricesA=GenerateMatrices();
            Matrix[] matricesB= GenerateMatrices();
            Task t1 =WriteMatricesToFileAsync(matricesA, matricesB, "Nurzhigit DIR");
            Task t2 =WriteMatricesToFileAsync(matricesB, matricesA, "NurzhigitJsonFormatDir");
            Task t3= WriteMatricesToFileBinaryAsync(matricesA, matricesB, "NurzhigitBinaryFormatDir");
            Task.WaitAll(t1, t2, t3);
            CompareMatrices();
            Console.ReadKey();
        }

        static void CreateDirectories()
        {
            string textFormatDir = "Нуржигит SEST-3-22 Основная папка";
            string jsonFormatDir = "Нуржигит SEST-3-22 JSON";
            string binaryFormatDir = "Нуржигит SEST-3-22 binary двоичный формат Dir";

            DeleteIfExists(textFormatDir);
            DeleteIfExists(jsonFormatDir);
            DeleteIfExists(binaryFormatDir);

            Directory.CreateDirectory(textFormatDir);
            Directory.CreateDirectory(jsonFormatDir);
            Directory.CreateDirectory(binaryFormatDir);
        }

        static void DeleteIfExists(string directory)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        static Matrix[] GenerateMatrices()
        {
            Matrix[] matrices = new Matrix[50];
            Random rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                double[,] values = new double[500, 100];

                for (int j = 0; j < 500; j++)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        values[j, k]=rand.NextDouble();
                    }
                }

                matrices[i]=new Matrix(values);
            }
            return matrices;
        }

        static async Task WriteMatricesToFileAsync(Matrix[] matricesA, Matrix[] matricesB, string directory)
        {
            for (int i = 0; i < 50; i++)
            {
                string filePath = Path.Combine(directory, $"Result{i}.tsv");
                await awaitWriteProductToFileAsync(matricesA[i], matricesB[i], filePath);
            }
        }
        static async Task WriteMatricesToFileBinaryAsync(Matrix[] matricesA, Matrix[] matricesB, string directory)
            {
                for (int i = 0; i < 50; i++)
                {
                    string filePath = Path.Combine(directory, $"Result{i}.dat");
                    await WriteMatricesToBinaryFileAsync(matricesA[i], matricesB[i], filePath);
                }
            }

            static async Task WriteMatricesToBinaryFileAsync(Matrix a, Matrix b, string filePath)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    byte[] matrixDataA = ConvertMatrixToByteArray(a);
                    byte[] matrixDataB = ConvertMatrixToByteArray(b);

                    writer.Write(matrixDataA.Length);
                    writer.Write(matrixDataA);
                    writer.Write(matrixDataB.Length);
                    writer.Write(matrixDataB);
                }
            }

            static byte[] ConvertMatrixToByteArray(Matrix matrix)
            {
                string matrixAsString = SerializeMatrixToString(matrix);
                byte[] matrixData = Encoding.UTF8.GetBytes(matrixAsString);
                return matrixData;
            }

            static string SerializeMatrixToString(Matrix matrix)
            {
                return matrix.ToString();
            }
            class MatrixOperations
            {
                public static string Multiply(Matrix a, Matrix b)
                {
                
                    return "Matrix multiplication result";
                }
            }




        
        static void WriteMatrix(this BinaryWriter writer, Matrix matrix)
        {
            writer.Write(matrix.Rows);
            writer.Write(matrix.Columns);
                for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    writer.Write(matrix[i, j]);
                }
            }
        }

        static void CompareMatrices()
        {
            string textFormatDir = "Нуржигит SEST-3-22 Основная папка";
            string jsonFormatDir = "Нуржигит SEST-3-22 JSON";

            for (int i=0; i < 50; i++)
            {
                string textFilePathA = Path.Combine(textFormatDir, $"Result{i}.tsv");
                string textFilePathB = Path.Combine(jsonFormatDir, $"Result{i}.json");

                Matrix matrixA = ReadMatrixFromFileAsync(textFilePathA).Result;
                Matrix matrixB = ReadMatrixFromFileAsync(textFilePathB).Result;

                bool isEqual = matrixA.Equals(matrixB);
                Console.WriteLine($"Are matrices {i} equal? {isEqual}");
            }
        }
        static async Task<Matrix> ReadMatrixFromFileAsync(string filePath)
        {
            Matrix matrix;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string[] dimensions=(await reader.ReadLineAsync()).Split(' ');
                int rows = int.Parse(dimensions[0]);
                int cols = int.Parse(dimensions[1]);
                double[,] values = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    string[] line = (await reader.ReadLineAsync()).Split('\t');
                    for (int j = 0; j < cols; j++)
                    {
                        values[i, j] = double.Parse(line[j]);
                    }
                }
                matrix = new Matrix(values);
            }
            return matrix;
        }
    }
}
