using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoSpeedControl.Model
{
    public class Settings
    {
        public static string SettingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"settings.json");

        public Settings()
        {            
            StartTime = new TimeSpan(9, 0, 0);
            EndTime = new TimeSpan(18, 0, 0);
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
