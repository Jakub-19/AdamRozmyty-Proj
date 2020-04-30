using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSSI
{
    public static class ElementaryFunctions
    {

        public static uint BlackColor { get; set; }
        public static uint WhiteColor { get; set; }
        public static double P { get; set; }
        //Odczyt()
        static public double Reading(int readingLeft, int readingRight) //funkcja zwracająca obliczoną wartość odczytu
        {
            return 0;
        }
        static public double Bell(double x, double c, double x1, double x2, bool straight) //funkcja zwracająca obliczoną wartość funkcji przynależności (wartości w zakresie <0, 1>)
        {
            if (x < x1 || x > x2)
                return 0;
            double a = 2.5, b = 2.5;
            if (straight)
            {
                a = 1.5; 
                b = 2.5;
            }
            return 1 / (1 + Math.Pow(Math.Abs((x - c) / a), 2 * b));
        }
        #region funkcje przynależności prędkości
        static public double SpeedSlow(double x) 
        {
            return Bell(x, 0, 0, 5, false);
        }
        static public double SpeedMedium(double x)
        {
            return Bell(x, 5, 0, 10, false);
        }
        static public double SpeedFast(double x)
        {
            return Bell(x, 10, 5, 15, false);
        }
        #endregion
        #region funkcje przynależności odczytu
        static public double ReadingEasyRight(double x)
        {
            return Bell(x, 5, 0, 10, false);
        }
        static public double ReadingHardRight(double x)
        {
            return Bell(x, 10, 5, 15, false);
        }
        static public double ReadingEasyLeft(double x)
        {
            return Bell(x, -5, -10, 0, false);
        }
        static public double ReadingHardLeft(double x)
        {
            return Bell(x, -10, -15, -5, false);
        }
        static public double ReadingStraight(double x)
        {
            return Bell(x, 0, -5, 5, true);
        }
        #endregion
    }
}
