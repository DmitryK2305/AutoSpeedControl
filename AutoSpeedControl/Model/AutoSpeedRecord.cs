using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSpeedControl.Model
{
    public class AutoSpeedRecord
    {
        public DateTime Time { get; set; }
        public string AutoNum { get; set; }
        public double Speed { get; set; }
    }
}
