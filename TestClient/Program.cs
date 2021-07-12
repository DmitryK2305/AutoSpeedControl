using AutoSpeedControl;
using Grpc.Core;
using System;
using System.Threading;
using static AutoSpeedControl.AutoSpeed;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:30051", ChannelCredentials.Insecure);

            var client = new AutoSpeed.AutoSpeedClient(channel);

            var rnd = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                var n = new int[5];

                for (int j = 0; i < 5; i++)
                    n[j] = rnd.Next(0, 10);

                Add(client, DateTime.Now, $"{n[0]}{n[1]}{n[2]}{n[3]} РР-{n[4]}", rnd.Next(10, 150));

                Console.WriteLine($"Запрос на превышение:\n{RequestSpeeding(client, DateTime.Now, 90)}");
                Console.WriteLine($"Запрос на минмакс:\n{RequestMinMax(client, DateTime.Now)}");
                Thread.Sleep(2000);
            }

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static void Add(AutoSpeedClient client, DateTime time, string num, double speed)
        {
            var reply = client.Add(new StringRequest { Data = $"- Дата и время фиксации 				{time:dd.MM.yyyy hh:mm:ss}\n- Номер транспортного средства 			{num}\n- Скорость движения км/ч 				{speed:0.0}" });
            //Console.WriteLine($"Reply: {reply.Success}");
        }

        public static string RequestSpeeding(AutoSpeedClient client, DateTime date, double speed)
        {
            var result = client.ReportSpeeding(new StringRequest() { Data = $"{date:dd.MM.yyyy}\n{speed}" });
            if (result.Success)
                return result.Data;
            else
                return "Не удалось запросить.";
        }

        public static string RequestMinMax(AutoSpeedClient client, DateTime date)
        {
            var result = client.ReportMinMax(new StringRequest() { Data = $"{date:dd.MM.yyyy}\nДата" });
            if (result.Success)
                return result.Data;
            else
                return "Не удалось запросить.";
        }
    }
}
