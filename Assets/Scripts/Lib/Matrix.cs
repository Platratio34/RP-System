/// <summary>
/// Mathematical matrix
/// </summary>
public class Matrix {
    /// <summary>
    /// content's of matrix
    /// </summary>
    public float[,] arr;
    /// <summary>
    /// Height of matrix (1st dimension)
    /// </summary>
    public int h;
    /// <summary>
    /// Width of matrix (2nd dimension)
    /// </summary>
    public int w;

    /// <summary>
    /// Create a new 0 matrix with height and width
    /// </summary>
    /// <param name="h">Width</param>
    /// <param name="w">Height</param>
    public Matrix(int h, int w) {
        this.h = h;
        this.w = w;
        arr = new float[h,w];
    }
    /// <summary>
    /// Create a new matrix from 2d array
    /// </summary>
    /// <param name="arr">Array</param>
    public Matrix(float[,] arr) {
        this.arr = arr;
        h = arr.GetLength(0);
        w = arr.GetLength(1);
    }

    /// <summary>
    /// Returns the dot product of this, and another matrix
    /// </summary>
    /// <param name="m">Other matrix</param>
    /// <returns>Dot product of the 2 matrices</returns>
    public Matrix Multiply(Matrix m) {
        Matrix o = new Matrix(h, m.w);
        
        for(int i = 0; i < h; i++) {
            for(int j = 0; j < m.w; j++) {
                float t = 0;
                for(int k = 0; k < w; k++) {
                    t += arr[i,k] * m.arr[k,j];
                }
                o.arr[i,j] = t;
            }
        }
        
        return o;
    }
}
