using Discord;
using Discord.WebSocket;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

namespace AminoBot
{
    public class Utils
    {

        public const string APP_ROOT = "app_config";





        public static void InitText()
        {
            aboutText = $"The AminoBot Project is a Discord Bot dedicated to being an Amino toolkit in the discord bot Format\n" +
                        $"The AminoBot Project is made in the C# Programming language and is powered by **Amino.NET**(*<https://github.com/FabioGaming/Amino.Net>*)\n" +
                        $"This bot has been developed and made possible by **@fabiogaming**(*1071594003717423134*)";

        }

        public Task CheckFiles()
        {
            if(!Directory.Exists(APP_ROOT)) { CreateFiles(); }
            if(!File.Exists($"{APP_ROOT}/config.json")) { CreateFiles(); }
            if (!File.Exists($"{APP_ROOT}/webDevs.txt")) { CreateFiles(); }
            return Task.CompletedTask;
        }

        private Task CreateFiles()
        {
            Directory.CreateDirectory(APP_ROOT);
            using (StreamWriter sw = File.CreateText($"{APP_ROOT}/config.json"))
            {
                sw.Write(JsonSerializer.Serialize(new Objects.Config(), options: new() { WriteIndented = true }));
            }
            File.CreateText($"{APP_ROOT}/webDevs.txt").Close();
            return Task.CompletedTask;
        }

        public Objects.Config GetConfig() => JsonSerializer.Deserialize<Objects.Config>(File.ReadAllText($"{APP_ROOT}/config.json"))!;

    }
}
