using System;
using System.IO;

namespace Application
{
    public class Gauss
    {
        //поля
        public uint RowCount;   //количество строк
        public uint ColumCount; //количество столбцов
        //свойства
        public double[][] Matrix { get; set; }  //матрица. левая часть системы уравнении в ввиде матрицы
        public double[] RightPart { get; set; } //правая часть системы уравнении
        public double[] Answer { get; set; }    //ответ
        //метод конструктор. для присвоения значения полям класса
        public Gauss(uint Row, uint Colum)
        {
            RightPart = new double[Row];
            Answer = new double[Row];
            Matrix = new double[Row][];
            for (int i = 0; i < Row; i++)
                Matrix[i] = new double[Colum];
            RowCount = Row;
            ColumCount = Colum;

            //обнулим массив
            for (int i = 0; i < Row; i++)
            {
                Answer[i] = 0;
                RightPart[i] = 0;
                for (int j = 0; j < Colum; j++)
                    Matrix[i][j] = 0;
            }
        }

        //выводим на экран матрицу
        public void PrintMatrix()
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix[i].Length; j++)
                {
                    Console.Write("{0,8:f2}", Matrix[i][j]);
                }
                Console.WriteLine("{0,5}{1,8:f2}", "|", RightPart[i]);
            }
        }
        //сортируем строки
        private void SortRows(int SortIndex)
        {

            double MaxElement = Matrix[SortIndex][SortIndex];
            int MaxElementIndex = SortIndex;
            for (int i = SortIndex + 1; i < RowCount; i++)
            {
                if (Matrix[i][SortIndex] > MaxElement)
                {
                    MaxElement = Matrix[i][SortIndex];
                    MaxElementIndex = i;
                }
            }

            //теперь найден максимальный элемент ставим его на верхнее место
            if (MaxElementIndex > SortIndex)//если это не первый элемент
            {
                //временная переменная
                double Temp;

                Temp = RightPart[MaxElementIndex];
                RightPart[MaxElementIndex] = RightPart[SortIndex];
                RightPart[SortIndex] = Temp;

                //меняем местами строки
                for (int i = 0; i < ColumCount; i++)
                {
                    Temp = Matrix[MaxElementIndex][i];
                    Matrix[MaxElementIndex][i] = Matrix[SortIndex][i];
                    Matrix[SortIndex][i] = Temp;
                }
            }
        }
        //метод для решения матрицы
        public int SolveMatrix()
        {
            if (RowCount != ColumCount)
                return 1; //нет решения

            for (int i = 0; i < RowCount - 1; i++)
            {
                SortRows(i);
                for (int j = i + 1; j < RowCount; j++)
                {
                    if (Matrix[i][i] != 0) //если главный элемент не 0, то производим вычисления
                    {
                        double MultElement = Matrix[j][i] / Matrix[i][i];
                        for (int k = i; k < ColumCount; k++)
                            Matrix[j][k] -= Matrix[i][k] * MultElement;
                        RightPart[j] -= RightPart[i] * MultElement;
                    }
                    //для нулевого главного элемента просто пропускаем данный шаг
                }
            }

            //ищем решение
            //потому что мы привели матрицу к треугольному ввиду и ищем решение с последней строки
            for (int i = (int)(RowCount - 1); i >= 0; i--)
            {
                Answer[i] = RightPart[i];

                for (int j = (int)(RowCount - 1); j > i; j--)
                    Answer[i] -= Matrix[i][j] * Answer[j];

                if (Matrix[i][i] == 0)
                    if (RightPart[i] == 0)
                        return 2; //множество решений
                    else
                        return 1; //нет решения

                Answer[i] /= Matrix[i][i];

            }
            return 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //выводим на экран информацию о интерфейсе программы
            Console.WriteLine("To read matrix elements");
            Console.WriteLine("from the keyboard enter 1");
            Console.WriteLine("from the file, enter 2");
            Console.WriteLine("for filling by random numbers 3");
            Console.Write("N: ");
            //пользователь выберает способ считывания элементов матрицы
            int select = int.Parse(Console.ReadLine());
            switch (select)
            {
                //ввод с клавиатуры
                case 1:
                    {
                        //вводим размер матрицы
                        Console.WriteLine("Enter the number of dimension: ");
                        uint size = uint.Parse(Console.ReadLine());
                        //создаем экземпляр класса
                        Gauss gauss = new Gauss(size, size);
                        Console.WriteLine("Enter the elements of the matrix");
                        Console.WriteLine("Enter the left side of equation" +
                            "");
                        for (int i = 0; i < gauss.Matrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < gauss.Matrix[i].Length; j++)
                            {
                                //считываем с экрана и переобразуем в число.
                                Console.Write("Enter an item [{0},{1}]: ", i, j);
                                gauss.Matrix[i][j] = double.Parse(Console.ReadLine());
                            }
                        }
                        Console.WriteLine("Enter the right side");
                        for (int i = 0; i < size; i++)
                        {
                            //считываем с экрана и переобразуем в число.
                            Console.Write("Enter an item [{0}]: ", i);
                            gauss.RightPart[i] = double.Parse(Console.ReadLine());
                        }
                        //выводим на экран исходную матрицу и треугольную и ответ
                        Print(gauss);
                    }
                    break;
                case 2:
                    {
                        //вводим путь к файлу
                        Console.Write("Enter the path to the file: ");
                        string directory = Console.ReadLine();
                        //объявляем экземпляр класса для работы с файлами


                        StreamReader reader = new StreamReader(@directory);
                        //считываем количество строк
                        uint size = (uint)reader.ReadToEnd().Split('\n').Length;
                        reader = new StreamReader(directory);
                        Gauss gauss = new Gauss(size, size);
                        string line;
                        int k = 0;
                        //считываем из файла элементы 
                        while ((line = reader.ReadLine()) != null)
                        {
                            //в файле коэффициенты разделены пробелом и мы используя это переобразуем строку в массив чисел
                            string[] coefficients = line.Split(' ');
                            for (int i = 0; i < coefficients.Length - 1; i++)
                            {   //заполняем матрицу
                                gauss.Matrix[k][i] = double.Parse(coefficients[i]);
                            }
                            //заполняем правую часть системы уравнении
                            gauss.RightPart[k] = double.Parse(coefficients[coefficients.Length - 1]);
                            k++;
                        }
                        //закрываем поток считывания
                        reader.Close();
                        //выводим на экран матрицу и треугольную матрицу и ответы
                        Print(gauss);

                    }
                    break;
                case 3:
                    {
                        //объявляем экземпляр класса для генерации случайных чисел
                        Random random = new Random();
                        //присваем случайный размер
                        uint size = (uint)random.Next(3, 10);
                        Gauss gauss = new Gauss(size, size);
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                //заполняем матрицу случайными коэффициентами от 0 до 10
                                gauss.Matrix[i][j] = random.Next(0, 10);
                            }
                            //заполняем правую часть уравнения коэеффициентами от 0 до 10
                            gauss.RightPart[i] = random.Next(0, 10);
                        }
                        //выводим на экран результат работы
                        Print(gauss);
                    }
                    break;
            }

            Console.ReadKey();
        }

        private static void Print(Gauss gauss)
        {
            Console.WriteLine();
            Console.WriteLine("Source Matrix");
            gauss.PrintMatrix();
            gauss.SolveMatrix();
            Console.WriteLine("Triangular matrix");
            gauss.PrintMatrix();
            Console.WriteLine("Answer:");
            foreach (var item in gauss.Answer)
            {
                Console.WriteLine("{0:f2}", item);
            }
        }
    }
}


