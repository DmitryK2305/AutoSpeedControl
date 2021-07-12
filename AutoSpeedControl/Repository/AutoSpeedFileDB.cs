using AutoSpeedControl.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSpeedControl.Repository
{
    public class AutoSpeedFileDB : IAutoSpeedDB
    {
        private const string DayFormat = "dd.MM.yyyy";        
        public string RepositoryPath { get; set; }
        private readonly ConcurrentDictionary<string, object> fileLocks;

        public AutoSpeedFileDB(string path)
        {
            RepositoryPath = path;
            if (!Directory.Exists(RepositoryPath))
            {
                Directory.CreateDirectory(RepositoryPath);
            }

            fileLocks = new ConcurrentDictionary<string, object>();
            foreach (var file in Directory.GetFiles(RepositoryPath).Select(t => Path.GetFileNameWithoutExtension(t)))
                fileLocks.AddOrUpdate(file, new object(), (key, cur) => cur);
        }
        
        public bool Add(AutoSpeedRecord record)
        {
            var result = false;
            try
            {
                var dateString = record.Time.ToString(DayFormat);
                var filePath = Path.Combine(RepositoryPath, $"{dateString}.txt");
                if (!fileLocks.Keys.Contains(dateString))
                {
                    fileLocks.AddOrUpdate(dateString, new object(), (key, cur) => cur);
                }

                lock (fileLocks[dateString])
                {
                    File.AppendAllText(filePath, $"{record.Time:hh:mm:ss};{record.AutoNum};{record.Speed}\n");
                }

                result = true;
            }
            catch (Exception ex)
            { 
            
            }

            return result;
        }

        public IEnumerable<AutoSpeedRecord> OnDate(DateTime date)
        {
            var dateString = date.ToString(DayFormat);

            if (fileLocks.Keys.Contains(dateString))
            {
                var filePath = Path.Combine(RepositoryPath, $"{dateString}.txt");
                return new AutoSpeedFileCollection(filePath);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
