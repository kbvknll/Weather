using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Data
{
    public class DateTemperature
    {
        public double Tempa { get; set; }
        public string Day { get; set; }
        public string Info { get; set; }

        public override string ToString()
        {
            return $"{Tempa}°C";
        }
        public DateTemperature (string day, string info, double tempa)
        {
            Day = day;
            Info = info;
            Tempa = tempa;
        }
    }
}
