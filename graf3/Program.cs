using System.Diagnostics;

namespace graf3
{
    internal class Program
    {
        static void Main(string[] args)
        {

            LenMatrix matrix;
            int N = 11, l = N - 1;

            var sw = new Stopwatch(); //активируем таймер
            sw.Start(); //запускаем отсчёт
            System.GC.Collect(); // вызываем сборщик мусора что бы очистить память от лишнего
            var size = GC.GetTotalMemory(true); //считываем значение памяти
            matrix = LenMatrix.GenerateNewMatrix(40, 1, 1, 9); //генерируем матрицу с заданными параметрами
            System.GC.Collect(); //снова очищаем память от лишнего
            size = GC.GetTotalMemory(true) - size; //считаем память занятую деревом
            sw.Stop(); //останавливаем таймер
            var matrixMakeTime = (sw.Elapsed); //записываем время
            sw.Restart();
            string path = "D:\\4\\11.txt";
            string path2 = "D:\\4\\viev.txt";
            LenMatrix.CreateFileFromMatrix(matrix, path); //сохраняем матрицу в столбцовом формате
            //matrix = LenMatrix.MakeMatrixFromFile(path);
            matrix.PrintToFileFromMatrix(path2); // сохраняем в файл матрицу как матрицу
            Console.WriteLine($"size = {size}\n time = {matrixMakeTime}\n");
            matrix.print(); // вывести матрицу
            sw.Start(); //запускаем отсчёт
            int answ = matrix.FindDeterminant(0, 1, new List<int>()); //считаем детерминант
            sw.Stop(); //останавливаем таймер
            var detTime = (sw.Elapsed); //записываем время
            Console.WriteLine();
            Console.WriteLine($"time = {detTime}\n"); // вывод времени работы расчёта определителя
            Console.WriteLine(answ); // вывод определителя
        }

    }
}
