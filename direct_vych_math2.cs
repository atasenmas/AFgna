using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1: x+0.5");//выводим перечень функций
            Console.WriteLine("2: x^2+12x+7.5");
            Console.WriteLine("3: x^3-5");
            Console.WriteLine("4: sin(x)");
            Console.WriteLine("5: x*cos(x)");
            int ch;//переменная для выбора функции
            FunkDelegate funk=Funk1;//делегат функции
            do
            {
                Console.WriteLine("Select an option");//выводим подсказку для выбора функции
                ch = Convert.ToInt32(Console.ReadLine());//выбираем функцию
                switch (ch)
                {
                    case 1: funk = Funk1; break;//в зависимости от выбора назначаем функцию делегату
                    case 2: funk = Funk2; break;
                    case 3: funk = Funk3; break;
                    case 4: funk = Funk4; break;
                    case 5: funk = Funk5; break;
                    default: Console.WriteLine("Invalid function selection"); break;//если выбрали цифру, которой не соответствует никакая функция
                }
            } while ((ch < 1) || (ch > 5));//повторяем пока не выберем нормальную функцию
            Console.WriteLine("Lower limit of integration");//вводим верхний, нижний пределы и точность интегрирования
            double a = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Upper integration limit");
            double b = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Required accuracy");
            double eps = Convert.ToDouble(Console.ReadLine());
            double[] result = PramLeft(funk, a, b, eps);//интегрируем левыми прямоугольниками
            Console.WriteLine("Methos of left rectangles");//показываем результаты
            Console.WriteLine("Value: "+result[0]);
            Console.WriteLine("Number of partitions: "+result[1]);
            Console.WriteLine("The accuracy obtained: "+result[2]);
            result = PramRight(funk, a, b, eps);//интегрируем правыми прямоугольниками
            Console.WriteLine("Methos of rigth rectangles");//показываем результаты
            Console.WriteLine("Value: " + result[0]);
            Console.WriteLine("Number of partitions: " + result[1]);
            Console.WriteLine("The accuracy obtained: " + result[2]);
            result = PramMiddle(funk, a, b, eps);//интегрируем средними прямоугольниками
            Console.WriteLine("Methos of middle rectangles");//показываем результаты
            Console.WriteLine("Value: " + result[0]);
            Console.WriteLine("Number of partitions: " + result[1]);
            Console.WriteLine("The accuracy obtained: " + result[2]);
            Console.ReadLine();
        }

        static double[] PramLeft(FunkDelegate funk, double aa, double bb,double eps)//левые прямоугольники
        {
            double a,b;//переменные для границ интегрирования
            if (bb>aa){a=aa;b=bb;}//проверка на то, что верхняя граница меньше нижней
            else {a=bb;b=aa;}
            double r1, r2,h;//два результата интегрирования с разными разбиениями и шаг интегрирования
            int n=10;//число разбиений
            r2 = 0;//обнуляем перед суммированием
            h = (b - a) / n;//считаем шаг
            for (int i = 0; i < n; i++) r2 += funk(a + i * h) * h;//суммируем по промежутками методом левых прямоугольников (по формуле)
            r1 = r2 - 10 * eps;//задаем начальное значение чтобы наверняка войти в цикл
            while (Math.Abs(r2 - r1) / 3 > eps)//пока не достигнем нужной точности
            {
                r1 = r2;//запоминаем предыдущее значение
                r2 = 0;//зануляем перед суммированием
                n *= 2;//увеличиваем разбиение в два раза
                h = (b - a) / n;//считаем шаг для разбиения
                for (int i = 0; i < n; i++) r2 += funk(a + i * h) * h;//суммируем по промежутками методом левых прямоугольников (по формуле)
            }
            double[] res = new double[3];//массив для вывода всех результатов
            res[0] = r2;//результат интегрирования
            res[1] = n;//число разбиений
            res[2] = Math.Abs(r2 - r1) / 3;//достигнутая точность
            return res;
        }

        static double[] PramRight(FunkDelegate funk, double aa, double bb, double eps)//правые прямоугольники
        {
            double a, b;//переменные для границ интегрирования
            if (bb > aa) { a = aa; b = bb; }//проверка на то, что верхняя граница меньше нижней
            else { a = bb; b = aa; }
            double r1, r2, h;//два результата интегрирования с разными разбиениями и шаг интегрирования
            int n = 10;//число разбиений
            r2 = 0;//обнуляем перед суммированием
            h = (b - a) / n;//считаем шаг
            for (int i = 1; i <= n; i++) r2 += funk(a + i * h) * h;//суммируем по промежутками методом правых прямоугольников (по формуле)
            r1 = r2 - 10 * eps;//задаем начальное значение чтобы наверняка войти в цикл
            while (Math.Abs(r2 - r1) / 3 > eps)//пока не достигнем нужной точности
            {
                r1 = r2;//запоминаем предыдущее значение
                r2 = 0;//зануляем перед суммированием
                n *= 2;//увеличиваем разбиение в два раза
                h = (b - a) / n;//считаем шаг для разбиения
                for (int i = 1; i <= n; i++) r2 += funk(a + i * h) * h;//суммируем по промежутками методом правых прямоугольников (по формуле)
            }
            double[] res = new double[3];//массив для вывода всех результатов
            res[0] = r2;//результат интегрирования
            res[1] = n;//число разбиений
            res[2] = Math.Abs(r2 - r1) / 3;//достигнутая точность
            return res;
        }

        static double[] PramMiddle(FunkDelegate funk, double aa, double bb, double eps)//средние прямоугольники
        {
            double a, b;//переменные для границ интегрирования
            if (bb > aa) { a = aa; b = bb; }//проверка на то, что верхняя граница меньше нижней
            else { a = bb; b = aa; }
            double r1, r2, h;//два результата интегрирования с разными разбиениями и шаг интегрирования
            int n = 10;//число разбиений
            r2 = 0;//обнуляем перед суммированием
            h = (b - a) / n;//считаем шаг
            for (int i = 0; i < n; i++) r2 += funk(a + (i + 0.5) * h) * h;//суммируем по промежутками методом средних прямоугольников (по формуле)
            r1 = r2 - 10 * eps;//задаем начальное значение чтобы наверняка войти в цикл
            while (Math.Abs(r2 - r1) / 3 > eps)//пока не достигнем нужной точности
            {
                r1 = r2;//запоминаем предыдущее значение
                r2 = 0;//зануляем перед суммированием
                n *= 2;//увеличиваем разбиение в два раза
                h = (b - a) / n;//считаем шаг для разбиения
                for (int i = 0; i < n; i++) r2 += funk(a + (i+0.5) * h) * h;//суммируем по промежутками методом средних прямоугольников (по формуле)
            }
            double[] res = new double[3];//массив для вывода всех результатов
            res[0] = r2;//результат интегрирования
            res[1] = n;//число разбиений
            res[2] = Math.Abs(r2 - r1) / 3;//достигнутая точность
            return res;
        }

        static double Funk1(double X)//функции для интегрирования
        {
            return X + 0.5;
        }

        static double Funk2(double X)
        {
            return X * X + 12 * X + 7.5;
        }

        static double Funk3(double X)
        {
            return X * X * X -5;
        }

        static double Funk4(double X)
        {
            return Math.Sin(X);
        }

        static double Funk5(double X)
        {
            return X * Math.Cos(X);
        }

        delegate double FunkDelegate(double X);//делегат
    }
}
