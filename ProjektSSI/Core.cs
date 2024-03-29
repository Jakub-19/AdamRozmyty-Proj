﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProjektSSI
{
    public static class Core
    {
        public static double HowTheRouteRuns(double speed, double reading)
        {
            const int hardLeft = -100;
            const int easyLeft = -50;
            const int straight = 0;
            const int easyRight = 50;
            const int hardRight = 100;

            speed /= 10;
            #region HardLeft
            double hardLeftMembership;
            double temp;
            temp = ElementaryFunctions.ReadingHardLeft(reading) * ElementaryFunctions.SpeedFast(speed);
            hardLeftMembership = temp;
            temp = ElementaryFunctions.ReadingHardLeft(reading) * ElementaryFunctions.SpeedMedium(speed);
            if (temp > hardLeftMembership)
                hardLeftMembership = temp;
            temp = ElementaryFunctions.ReadingEasyLeft(reading) * ElementaryFunctions.SpeedFast(speed);
            if (temp > hardLeftMembership)
                hardLeftMembership = temp;
            temp = ElementaryFunctions.ReadingHardLeft(reading) * ElementaryFunctions.SpeedSlow(speed);
            if (temp > hardLeftMembership)
                hardLeftMembership = temp;
            #endregion

            #region EasyLeft
            double easyLeftMembership;
            temp = ElementaryFunctions.ReadingEasyLeft(reading) * ElementaryFunctions.SpeedMedium(speed);
            easyLeftMembership = temp;
            temp = ElementaryFunctions.ReadingEasyLeft(reading) * ElementaryFunctions.SpeedSlow(speed);
            if(temp > easyLeftMembership)
                easyLeftMembership = temp;
            #endregion

            #region straighttraight
            double straightMembership;
            temp = ElementaryFunctions.ReadingStraight(reading) * ElementaryFunctions.SpeedFast(speed);
            straightMembership = temp;
            temp = ElementaryFunctions.ReadingStraight(reading) * ElementaryFunctions.SpeedMedium(speed);
            if (temp > straightMembership)
                straightMembership = temp;
            temp = ElementaryFunctions.ReadingStraight(reading) * ElementaryFunctions.SpeedSlow(speed);
            if (temp > straightMembership)
                straightMembership = temp;
            #endregion

            #region HardRight
            double hardRightMembership;
            temp = ElementaryFunctions.ReadingHardRight(reading) * ElementaryFunctions.SpeedFast(speed);
            hardRightMembership = temp;
            temp = ElementaryFunctions.ReadingHardRight(reading) * ElementaryFunctions.SpeedMedium(speed);
            if (temp > hardRightMembership)
                hardRightMembership = temp;
            temp = ElementaryFunctions.ReadingEasyRight(reading) * ElementaryFunctions.SpeedFast(speed);
            if (temp > hardRightMembership)
                hardRightMembership = temp;
            temp = ElementaryFunctions.ReadingHardRight(reading) * ElementaryFunctions.SpeedSlow(speed);
            if (temp > hardRightMembership)
                hardRightMembership = temp;
            #endregion

            #region EasyRight
            double easyRightMembership;
            temp = ElementaryFunctions.ReadingEasyRight(reading) * ElementaryFunctions.SpeedMedium(speed);
            easyRightMembership = temp;
            temp = ElementaryFunctions.ReadingEasyRight(reading) * ElementaryFunctions.SpeedSlow(speed);
            if (temp > easyRightMembership)
                easyRightMembership = temp;
            #endregion

            double value = (easyLeftMembership * easyLeft + easyRightMembership * easyRight + hardRightMembership * hardRight + hardLeftMembership * hardLeft + straightMembership * straight) / (easyLeftMembership + easyRightMembership + hardRightMembership + hardLeftMembership + straightMembership);
            return value;
        }
    }
}
