using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoSpeedControl.Extensions;
using AutoSpeedControl.Model;
using AutoSpeedControl.Repository;
using Grpc.Core;

namespace AutoSpeedControl
{
    public class AutoSpeedImpl : AutoSpeed.AutoSpeedBase
    {
        IAutoSpeedDB dataBase;
        Settings settings;

        public AutoSpeedImpl(IAutoSpeedDB dataBase, Settings settings)
        {
            this.dataBase = dataBase;
            this.settings = settings;
        }

        public override Task<SuccessReply> Add(StringRequest request, ServerCallContext context)
        {
            return Task.Run(() => {
                var success = false;
                try
                {
                    var dateString = Regex.Match(request.Data, @"(\d{2}).(\d{2}).(\d{4}) (\d{2}):(\d{2}):(\d{2})").Value;
                    var num = Regex.Match(request.Data, @"(\d{4}) (\w{2})-(\d{1})").Value;
                    var speed = Regex.Match(request.Data, @"\d+,\d").Value;
                    var record = new AutoSpeedRecord()
                    {
                        Time = DateTime.Parse(dateString),
                        AutoNum = num,
                        Speed = double.Parse(speed)
                    };
                    success = dataBase.Add(record);
                }
                catch (Exception ex)
                { 
                
                };
                
                return new SuccessReply { Success = success };
                });
        }

        public override Task<StringReply> ReportSpeeding(StringRequest request, ServerCallContext context)
        {            
            return Task.Run(() => {
                var now = DateTime.Now;
                var timeNow = new TimeSpan(now.Hour, now.Minute, 0);                
                if (timeNow >= settings.StartTime && timeNow <= settings.EndTime)
                {
                    var result = new StringReply();
                    try
                    {
                        var requestLines = request.Data.Split(new[] { '\r', '\n' });
                        var data = DateTime.Parse(requestLines[0]);
                        var speed = double.Parse(requestLines[1]);
                        var records = dataBase.OnDate(data).Where(rec => rec.Speed > speed);
                        result.Data = records.StringRepresentation();
                        result.Success = true;
                    }
                    catch { }

                    return result;
                }
                else
                {
                    return new StringReply() { Success = false };
                }
            });
        }

        public override Task<StringReply> ReportMinMax(StringRequest request, ServerCallContext context)
        {
            return Task.Run(() => {
                var now = DateTime.Now;
                var timeNow = new TimeSpan(now.Hour, now.Minute, 0);
                if (timeNow >= settings.StartTime && timeNow <= settings.EndTime)
                {
                    var result = new StringReply();
                    try
                    {
                        var requestLines = request.Data.Split(new[] { '\r', '\n' });
                        var data = DateTime.Parse(requestLines[0]);
                        var minSpeedRecord = dataBase.OnDate(data).Min(rec => rec.Speed);
                        var maxSpeedRecord = dataBase.OnDate(data).Max(rec => rec.Speed);
                        result.Data = $"{minSpeedRecord}\n{maxSpeedRecord}";
                        result.Success = true;
                    }
                    catch { }

                    return result;
                }
                else
                {
                    return new StringReply() { Success = false };
                }
            });
        }
    }
}
