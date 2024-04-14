using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;

namespace AminoBot
{
    public class Utils
    {
        public static string helpText;
        public static string aboutText;



        public static string botToken;
        public static ulong logChannel;
        public static Amino.Client mainClient;
        public static string clientEmail;
        public static string clientPassword;
        public static string clientDevice;


        public static List<string> webDevices = new List<string>();



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



        public static void CreateConfig()
        {
            if(!File.Exists("Aminobot/config.json"))
            {
                JObject json = new JObject();
                json.Add("botToken", String.Empty);
                json.Add("logChannel", 0);
                json.Add("accountEmail", "");
                json.Add("accountPassword", "");
                json.Add("accountDeviceId", "");
                Directory.CreateDirectory("Aminobot");
                using (StreamWriter sw = File.CreateText("AminoBot/config.json"))
                {
                    sw.Write(json);
                }
                File.CreateText("Aminobot/webDevs.txt");
            }
        }

        public static void ReadConfig()
        {
            string data = File.ReadAllText("Aminobot/config.json");
            botToken = (string)JObject.Parse(data)["botToken"];
            logChannel = Convert.ToUInt64(JObject.Parse(data)["logChannel"]);
            clientEmail = (string)JObject.Parse(data)["accountEmail"];
            clientPassword = (string)JObject.Parse(data)["accountPassword"];
            clientDevice = (string)JObject.Parse(data)["accountDeviceId"];

            webDevices = File.ReadAllLines("Aminobot/webDevs.txt").ToList();
        }


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
        public class ExtraGen
        {
            static T[] CombineTwoArrays<T>(T[] a1, T[] a2)
            {
                T[] arrayCombined = new T[a1.Length + a2.Length];
                Array.Copy(a1, 0, arrayCombined, 0, a1.Length);
                Array.Copy(a2, 0, arrayCombined, a1.Length, a2.Length);
                return arrayCombined;
            }


            static byte[] StringToByteArray(string hex)
            {
                return Enumerable.Range(0, hex.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                 .ToArray();
            }

            public static string GetWebDevice()
            {
                try
                {
                    Random rdm = new Random();
                    string dev = webDevices[rdm.Next(webDevices.Count)];

                    webDevices.Remove(dev);
                    using (StreamWriter sw = File.CreateText("Aminobot/webDevs.txt"))
                    {
                        foreach(string device in webDevices)
                        {
                            sw.WriteLine(device);
                        }
                    }

                    if(webDevices.Count < 100) { _ = Task.Run(async () => { warnCount(); }); }

                    return dev;
                }
                catch { throw new Exception(); }
            }

            public static void checkCount()
            {
                if (webDevices.Count < 100) { _ = Task.Run(async () => { warnCount(); }); }
            }

            private static async void warnCount()
            {
                try
                {
                    ITextChannel channel = (ITextChannel)Program.client.GetChannel(Convert.ToUInt64(Utils.logChannel));
                    await channel.SendMessageAsync("", false, Templates.Embeds.webDeviceCountWarn(webDevices.Count).Build());
                }
                catch { }

            }



            public static string deviceId(int prefixMode = 19)
            {
                string prefix = "19";
                string key = "E7309ECC0953C6FA60005B2765F99DBBC965C8E9";
                switch (prefixMode)
                {
                    case 18:
                        prefix = "18";
                        key = "d19d2cb8468aac9b0ae16be4a6fa464be63760ce";
                        break;
                    case 19:
                        prefix = "19";
                        key = "E7309ECC0953C6FA60005B2765F99DBBC965C8E9";
                        break;
                    case 22:
                        prefix = "22";
                        key = "307c3c8cd389e69dc298d951341f88419a8377f4";
                        break;
                    case 32:
                        prefix = "32";
                        key = "76b4a156aaccade137b8b1e77b435a81971fbd3e";
                        break;
                    case 42:
                        prefix = "42";
                        key = "02b258c63559d8804321c5d5065af320358d366f";
                        break;
                    case 52:
                        prefix = "52";
                        key = "AE49550458D8E7C51D566916B04888BFB8B3CA7D";
                        break;

                }
                Random rnd = new Random();
                byte[] identifier = new byte[20];
                rnd.NextBytes(identifier);
                HMACSHA1 hmac = new HMACSHA1(StringToByteArray(key));
                byte[] buffer = CombineTwoArrays(StringToByteArray(prefix), identifier);
                string result = BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
                return prefix + BitConverter.ToString(identifier).Replace("-", "").ToLower() + result;
            }
        }


        public class CoolDown
        {
            static Dictionary<ulong, int> timeOutList = new Dictionary<ulong, int>();

            public static bool isCooldown(ulong userId)
            {
                if (timeOutList.ContainsKey(userId)) { return true; } else { return false; }
            }

            public static async Task addUser(ulong userId)
            {
                _ = Task.Run(async () =>
                {
                    int waitTime = 30;
                    timeOutList.Add(userId, waitTime);

                    for(int i = waitTime; i > 0; i--)
                    {
                        timeOutList[userId] = i;
                        await Task.Delay(1000);
                    }
                    if(timeOutList.ContainsKey(userId)) { timeOutList.Remove(userId); }
                    
                });
            }
            public static async Task removeUser(ulong userId)
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    if (timeOutList.ContainsKey(userId)) { timeOutList.Remove(userId); }
                });
            }


            public static int getTimeoutSeconds(ulong userId)
            {
                if (timeOutList.ContainsKey(userId)) { return timeOutList[userId]; } else { return 0; }
            }
        }

    }
}
