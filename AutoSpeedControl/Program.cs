using AutoSpeedControl.Model;
using AutoSpeedControl.Repository;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace AutoSpeedControl
{
    public class Program
    {        
        const int Port = 30051;

        public static void Main(string[] args)
        {
            string settingsPath = Settings.SettingsPath;
            string repositoryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Repository");

            Settings settings;
            if (!File.Exists(settingsPath))
            {
                settings = new Settings();
                File.WriteAllText(settingsPath, JsonConvert.SerializeObject(settings));
            }
            else
            {
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsPath));
            }

            Server server = new Server
            {
                Services = { AutoSpeed.BindService(new AutoSpeedImpl(new AutoSpeedFileDB(repositoryPath), settings)) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Server started at " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
        
    }
}
