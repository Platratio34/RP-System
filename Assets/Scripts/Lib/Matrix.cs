
public class Matrix {
    public float[,] arr;
    public int h;
    public int w;

    public Matrix(int h, int w) {
        this.h = h;
        this.w = w;
        arr = new float[h,w];
    }
    public Matrix(float[,] arr) {
        this.arr = arr;
        h = arr.GetLength(0);
        w = arr.GetLength(1);
    }

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
