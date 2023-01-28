using Discord;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AminoBot
{
    public class Utils
    {
        public static string botToken;
        public static ulong logChannel;
        public static Amino.Client mainClient;
        public static string clientEmail;
        public static string clientPassword;
        public static string clientDevice;

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
                    timeOutList.Remove(userId);
                });
            }


            public static int getTimeoutSeconds(ulong userId)
            {
                if (timeOutList.ContainsKey(userId)) { return timeOutList[userId]; } else { return 0; }
            }
        }

    }
}
