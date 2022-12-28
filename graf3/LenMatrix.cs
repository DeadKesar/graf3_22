using graf3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graf3
{
    internal class LenMatrix
    {
        public int[] values; // массив значений
        public int[] rows; // массив строк
        public int[] pointers; // массив указателей
        public int l; //ширина полу полосы пропускания
        public int N; // размерность матрицы
        public int capasity() // число не нулевых элементов
        { return values.Length; }
        LenMatrix(int N, int len, int l = 0) //создание пустой матрицы
        {
            this.l = l;
            this.N = N;
            //capasity = 2 * N*l + N - l * l - l;
            values = new int[len];
            rows = new int[len];
            pointers = new int[N + 1];
        }
        LenMatrix(int[] values, int[] rows, int[] pointers, int N, int l) // создание заполненной матрицы
        {
            this.values = values;
            this.rows = rows;
            this.pointers = pointers;
            this.N = N;
            this.l = l;
        }

        public static LenMatrix MakeMatrixFromFile(string path) //создать матрицу из файла
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                int N = int.Parse(line);
                line = reader.ReadLine();
                int l = int.Parse(line);
                char[] separators = new char[] { ' ' };
                line = reader.ReadLine();
                var st = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                LenMatrix matrix = new LenMatrix(N, st.Length, l);
                for (int i = 0; i < st.Length; i++)
                {
                    matrix.values[i] = int.Parse(st[i]);
                }
                line = reader.ReadLine();
                st = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < st.Length; i++)
                {
                    matrix.rows[i] = int.Parse(st[i]);
                }
                line = reader.ReadLine();
                st = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < st.Length; i++)
                {
                    matrix.pointers[i] = int.Parse(st[i]);
                }
                return matrix;
            }
        }

        public int FindDeterminant(int start, int end, List<int> used) // поиск определителя
        {
            int det = 0;
            if (end != pointers.Length - 1)
            {
                for (int i = 1, j = pointers[start], count = 0; i < end + l + 1 && i <= N + 1 && j < pointers[end]; i++, count++)//end-l>0? end - l:0
                {
                    if (i < rows[j])
                    {
                        if (used.Contains(i))
                        {
                            count--;
                            continue;
                        }
                        continue;
                    }
                    if (used.Contains(i))
                    {
                        count--;
                        j++;
                        continue;
                    }
                    if (values[j] == 0)
                    {
                        j++;
                        continue;
                    }
                    if (i != rows[j])
                    {
                        i++;
                        continue;
                    }
                    used.Add(i);
                    det += values[j++] * (count % 2 == 0 ? 1 : -1) * FindDeterminant(start + 1, end + 1, used);// (int)Math.Pow(-1.0, end * rows[i] * g)
                    used.Remove(i);
                }
                return det;
            }
            else
            {
                for (int i = pointers[start]; i < pointers[end]; i++)
                {
                    if (!used.Contains(rows[i]))
                    {
                        return values[i];
                    }
                }
            }
            return 0;
        }

        public void print() // вывод матрицы
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < pointers.Length - 1; j++)
                {
                    bool isFind = false;
                    for (int start = pointers[j], end = pointers[j + 1]; start < end; start++)
                    {
                        if (this.rows[start] - 1 == i)
                        {
                            Console.Write($"{values[start]} ");
                            isFind = true;
                            break;
                        }
                    }
                    if (!isFind)
                        Console.Write("0 ");
                }
                Console.WriteLine();
            }
        }

        public static LenMatrix GenerateNewMatrix(int N, int l, int from, int to)
        {
            if (l >= N + 1)
            {
                Console.WriteLine("imposible");
                return null;
            }
            Random rand = new Random((int)DateTime.Now.Ticks);
            List<int> values = new List<int>();
            List<int> rows = new List<int>();
            List<int> pointers = new List<int>();
            // int  j = 0, k = l + 1, n = 2 * N + N - l * l + l;
            int row = 1;
            for (int col = 1; col <= N; col++)
            {
                bool isFirst = true;
                for (int i = 0 > col - (1 + l) ? 0 : col - (1 + l); i < col + l && i < N; i++)
                {
                    int temp = rand.Next(from, to);
                    if (temp != 0)
                    {
                        values.Add(temp);
                        rows.Add(row + i);
                        if (isFirst)
                        {
                            pointers.Add(rows.Count - 1);
                            isFirst = false;
                        }
                    }
                }
                if (isFirst)
                {
                    pointers.Add(rows.Count - 1);
                    isFirst = false;
                }
            }
            pointers.Add(rows.Count);

            return new LenMatrix(values.ToArray(), rows.ToArray(), pointers.ToArray(), N, l);
        }

        public void PrintToFileFromMatrix(string path) //запись в файл матрицы как матрицы nxn
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < pointers.Length - 1; j++)
                    {
                        bool isFind = false;
                        for (int start = pointers[j], end = pointers[j + 1]; start < end; start++)
                        {
                            if (this.rows[start] - 1 == i)
                            {
                                writer.Write($"{values[start]} ");
                                isFind = true;
                                break;
                            }
                        }
                        if (!isFind)
                            writer.Write("0 ");
                    }
                    writer.WriteLine();
                }
            }
        }

        public static void CreateFileFromMatrix(LenMatrix mat, string path) //создать матрицу из файла
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(mat.N);
                writer.WriteLine(mat.l);
                StringBuilder stb = new StringBuilder();
                foreach (int a in mat.values)
                {
                    stb.Append($"{a} ");
                }
                writer.Write(stb.ToString());
                writer.WriteLine();
                stb = new StringBuilder();
                foreach (int a in mat.rows)
                {
                    stb.Append($"{a} ");
                }
                writer.Write(stb.ToString());
                writer.WriteLine();
                stb = new StringBuilder();
                foreach (int a in mat.pointers)
                {
                    stb.Append($"{a} ");
                }
                writer.Write(stb.ToString());
                writer.WriteLine();
            }
        }
    }
}
