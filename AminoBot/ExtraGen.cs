using Discord;
using System.Security.Cryptography;

namespace AminoBot
{
    public static class ExtraGen
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
}
