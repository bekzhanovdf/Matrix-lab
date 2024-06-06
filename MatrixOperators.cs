public static class MatrixOperations
{
    public static Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
            throw new ArgumentException("Количество строк и колонок должны совподать.");

        int rows = a.Rows;
        int cols = b.Columns;
        int common = a.Columns;
        double[,] result = new double[rows, cols];
        for(int i=0;i<result.Length;i++ )
        {
            for (int j = 0; j < cols; j++)
            {
                double sum = 0;
                for (int k = 0; k < common; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        }
        return new Matrix(result);
    }
}
