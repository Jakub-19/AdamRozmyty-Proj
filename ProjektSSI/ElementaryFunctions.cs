using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProjektSSI
{
    public static class ElementaryFunctions
    {
        public static uint BlackColorLeft { get; set; }
        public static uint WhiteColorLeft { get; set; }
        public static uint BlackColorRight { get; set; }
        public static uint WhiteColorRight { get; set; }
        public static double P_L { get; set; }
        public static double P_R { get; set; }
        private static bool previousTurn = true;//true = prawo
        static public double Reading(int readingLeft, int readingRight) //funkcja zwracająca obliczoną wartość odczytu, gdzie 100-maksymalnie czarny, 0-maksymalnie biały
        {
            Debug.WriteLine("readingLeft: " + readingLeft);
            Debug.WriteLine("readingRight: " + readingRight);
            double readL = Math.Abs(((readingLeft - BlackColorLeft) * P_L) - 100);
            double readR = Math.Abs(((readingRight - BlackColorRight) * P_R) - 100);
            Debug.WriteLine("readL: " + readL);
            Debug.WriteLine("readR: " + readR);

            if (readingLeft > WhiteColorLeft/2 && readingRight > WhiteColorRight/2)
            {
                if (previousTurn)
                    return -10;
                else
                    return 10;
            }
            if (readL > readR)
                previousTurn = false;
            else
                previousTurn = true;

            return ((readR - readL) / 10);
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
        static public double SpeedRear(double x)
        {
            if (x >= 0)
                return 0;
            else
                return 1;
        }
        #endregion
        #region funkcje przynależności odczytu
        static public double ReadingEasyRight(double x)
        {
            return Bell(x, 4, 0, 10, false);
        }
        static public double ReadingHardRight(double x)
        {
            return Bell(x, 10, 5, 15, false);
        }
        static public double ReadingEasyLeft(double x)
        {
            return Bell(x, -4, -10, 0, false);
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
