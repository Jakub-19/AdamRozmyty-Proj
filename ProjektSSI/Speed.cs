using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProjektSSI
{
    public static class Speed
    {
        private static double Max(params double[] x) //funkcja zwracająca największa wartość z tablicy
        {
            double tmp = x[0];
            foreach (var a in x)
                if (a > tmp)
                    tmp = a;
            return tmp;
        }
        public static double GetResultantSpeed(int leftMotorSpeed, int rightMotorSpeed) //funkcja zwracająca prędkość wypadkową pojazdu
        {
            leftMotorSpeed /= 10;
            rightMotorSpeed /= 10;
            #region definicje zmiennych
            double leftMotorSpeedRear = ElementaryFunctions.SpeedRear(leftMotorSpeed);
            double leftMotorSpeedSlow = ElementaryFunctions.SpeedSlow(leftMotorSpeed);
            double leftMotorSpeedMedium = ElementaryFunctions.SpeedMedium(leftMotorSpeed);
            double leftMotorSpeedFast = ElementaryFunctions.SpeedFast(leftMotorSpeed);

            double rightMotorSpeedRear = ElementaryFunctions.SpeedRear(rightMotorSpeed);
            double rightMotorSpeedSlow = ElementaryFunctions.SpeedSlow(rightMotorSpeed);
            double rightMotorSpeedMedium = ElementaryFunctions.SpeedMedium(rightMotorSpeed);
            double rightMotorSpeedFast = ElementaryFunctions.SpeedFast(rightMotorSpeed);

            double[] tabSlow = new double[7];
            double[] tabMedium = new double[5];
            double[] tabFast = new double[3];
            #endregion

            //deklaracja stałych 
            const int slowParameter = 10;
            const int mediumParameter = 50;
            const int fastParameter = 90;

            //obliczenie wszystkich wartosci dla predkosci wypadkowej wolno
            tabSlow[0] = leftMotorSpeedRear * rightMotorSpeedSlow;
            tabSlow[1] = leftMotorSpeedRear * rightMotorSpeedMedium;
            tabSlow[2] = leftMotorSpeedRear * rightMotorSpeedFast;
            tabSlow[3] = leftMotorSpeedSlow * rightMotorSpeedRear;
            tabSlow[4] = leftMotorSpeedSlow * rightMotorSpeedSlow;
            tabSlow[5] = leftMotorSpeedMedium * rightMotorSpeedRear;
            tabSlow[6] = leftMotorSpeedFast * rightMotorSpeedRear;

            //obliczenie wszystkich wartosci dla predkosci wypadkowej średnio
            tabMedium[0] = leftMotorSpeedSlow * rightMotorSpeedMedium;
            tabMedium[1] = leftMotorSpeedSlow * rightMotorSpeedFast;
            tabMedium[2] = leftMotorSpeedMedium * rightMotorSpeedSlow;
            tabMedium[3] = leftMotorSpeedMedium * rightMotorSpeedMedium;
            tabMedium[4] = leftMotorSpeedFast * rightMotorSpeedSlow;

            //obliczenie wszystkich wartosci dla predkosci wypadkowej szybko
            tabFast[0] = leftMotorSpeedMedium * rightMotorSpeedFast;
            tabFast[1] = leftMotorSpeedFast * rightMotorSpeedMedium;
            tabFast[2] = leftMotorSpeedFast * rightMotorSpeedFast;

            //wybranie wartości max dla reguł pogrupowanych względem S-M-F
            double resultantSpeedSlow = Max(tabSlow);
            double resultantSpeedMedium = Max(tabMedium);
            double resultantSpeedFast = Max(tabFast);

            //defuzzyfikacja metodą środka ciężkości
            double result = ((resultantSpeedSlow * slowParameter) + (resultantSpeedMedium * mediumParameter) + (resultantSpeedFast * fastParameter)) 
                / (resultantSpeedSlow + resultantSpeedMedium + resultantSpeedFast);

            return result;
        }
    }
}
