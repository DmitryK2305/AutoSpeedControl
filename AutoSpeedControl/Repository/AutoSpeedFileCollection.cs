using AutoSpeedControl.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSpeedControl.Repository
{
    public class AutoSpeedFileCollection : IEnumerable<AutoSpeedRecord>
    {
        private readonly string filePath;

        public AutoSpeedFileCollection(string filePath)
        {
            this.filePath = filePath;
        }

        public IEnumerator<AutoSpeedRecord> GetEnumerator()
        {
            using StreamReader sr = new StreamReader(filePath);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                var splitedLine = line.Split(';');
                var time = DateTime.Parse($"{Path.GetFileNameWithoutExtension(filePath)} {splitedLine[0]}");
                var num = splitedLine[1];
                var speed = double.Parse(splitedLine[2]);
                var record = new AutoSpeedRecord() { Time = time, AutoNum = num, Speed = speed };
                yield return record;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
