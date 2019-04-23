using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    class TimeConverter
    {
        public String SecondsToHms(double time)
        {
            var h = Math.Floor(time / 3600);
            var m = Math.Floor(time % 3600 / 60);
            var s = Math.Floor(time % 3600 % 60);

            var hDisplay = "";
            var mDisplay = "";
            var sDisplay = "";

            //hours
            if (h > 0 && h < 10)
                hDisplay = "0" + h + ":";
            else if (h >= 10)
                hDisplay = h + ":";
            else
                hDisplay = "00:";

            //minutes
            if (m > 0 && m < 10)
                mDisplay = "0" + m + ":";
            else if (m >= 10)
                mDisplay = m + ":";
            else
                mDisplay = "00:";

            //seconds
            if (s > 0 && s < 10)
                sDisplay = "0" + s;
            else if (s >= 10)
                sDisplay = s + "";
            else
                sDisplay = "00";

            return hDisplay + mDisplay + sDisplay;
        }
    }
}
