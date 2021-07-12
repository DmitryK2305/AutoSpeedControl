using AutoSpeedControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSpeedControl.Repository
{
    public interface IAutoSpeedDB
    {
        bool Add(AutoSpeedRecord record);
        IEnumerable<AutoSpeedRecord> OnDate(DateTime date);        
    }
}
