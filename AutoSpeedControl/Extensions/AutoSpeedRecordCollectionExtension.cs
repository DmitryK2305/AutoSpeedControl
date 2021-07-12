using AutoSpeedControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSpeedControl.Extensions
{
    public static class AutoSpeedRecordCollectionExtension
    {
        public static string StringRepresentation(this IEnumerable<AutoSpeedRecord> collection)
        {
            var builder = new StringBuilder();
            foreach (var item in collection)
                builder.Append($"{item.Time:dd.MM.yyyy hh:mm:ss} {item.AutoNum} {item.Speed:0.0}\n");
            return builder.ToString();
        }
    }
}
