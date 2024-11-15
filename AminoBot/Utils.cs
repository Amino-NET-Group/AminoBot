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
            helpText = $"List of Commands:\n" +
                        $"**</amino-help:{Program.client.CurrentUser.Id}>** - Shows a Help / About menu\n" +
                        $"**</device:{Program.client.CurrentUser.Id}>** - Generates a Device ID\n" +
                        $"**</objectid:{Program.client.CurrentUser.Id}>** <URL> - Gets the Object ID of a specific Amino URL\n" +
                        $"**</communityid:{Program.client.CurrentUser.Id}>** <communityURL> | <Amino URL from post, chat or user in community> - Gets the Community ID of a given Amino URL\n" +
                        $"**</public-url:{Program.client.CurrentUser.Id}>** <user URL> - Gets the public profile URL of an Amino User from object ID or profile URL\n" +
                        $"**</device-extra:{Program.client.CurrentUser.Id}>** <prefix> - Allows you to generate a Device ID with a specific Prefix\n" +
                        $"**</verify:{Program.client.CurrentUser.Id}>** <deviceId> - Allows you to see if a device ID is valid or not\n" +
                        $"**</web-device:{Program.client.CurrentUser.Id}>** - Allows you to generate a prefix 17 Device ID\n " +
                        $"**</created-time:{Program.client.CurrentUser.Id}>** <user URL> - Allows you to see the Date and Time a user has joined Amino\n" +
                        $"**</show-profile:{Program.client.CurrentUser.Id}>** <user URL> - Allows you to get information about someones public Profile\n" +
                        $"**</show-community:{Program.client.CurrentUser.Id}>** <comUrl | comId | url> - Allows you to get information about a Community and its agent";

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


        public static async void deviceFileCheck(SocketMessage msg)
        {
            int WDC = 0;
            try
            {
                
                if (msg.Author.IsBot) { return; }
                if (msg.Author.IsWebhook) { return; }
                if (msg.Channel.Id == Utils.logChannel)
                {
                    if (msg.Attachments.Count > 0)
                    {
                        if (msg.Attachments.First().ContentType.Contains("text/plain"))
                        {
                            WebClient WClient = new WebClient();
                            WClient.DownloadFile(msg.Attachments.First().Url, "tempWebDevices.txt");
                            WDC = File.ReadAllLines("tempWebDevices.txt").Length;
                            foreach (string dev in File.ReadAllLines("tempWebDevices.txt")) { if (dev.StartsWith("17")) { webDevices.Add(dev.ToString()); } }
                            File.Delete("tempWebDevices.txt");
                            using (StreamWriter sw = File.CreateText("Aminobot/webDevs.txt"))
                            {
                                foreach (string device in webDevices)
                                {
                                    sw.WriteLine(device);
                                }
                            }
                            try
                            {
                                ITextChannel channel = (ITextChannel)Program.client.GetChannel(Convert.ToUInt64(Utils.logChannel));
                                await channel.SendMessageAsync("", false, Templates.Embeds.webDevicesAdded(WDC, webDevices.Count).Build());
                            }
                            catch { }
                            try { msg.DeleteAsync(); } catch { }
                        }
                    }
                }


            }
            catch { }

        }

    }
}
