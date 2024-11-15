using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot
{
    public static class CoolDown
    {
        static Dictionary<ulong, int> timeOutList = new Dictionary<ulong, int>();

        public static bool IsCooldown(ulong userId)
        {
            if (timeOutList.ContainsKey(userId)) { return true; } else { return false; }
        }

        public static async Task AddUser(ulong userId)
        {
            _ = Task.Run(async () =>
            {
                int waitTime = 30;
                timeOutList.Add(userId, waitTime);

                for (int i = waitTime; i > 0; i--)
                {
                    timeOutList[userId] = i;
                    await Task.Delay(1000);
                }
                if (timeOutList.ContainsKey(userId)) { timeOutList.Remove(userId); }

            });
        }
        public static async Task RemoveUser(ulong userId)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(1000);
                if (timeOutList.ContainsKey(userId)) { timeOutList.Remove(userId); }
            });
        }


        public static int GetTimeoutSeconds(ulong userId)
        {
            if (timeOutList.ContainsKey(userId)) { return timeOutList[userId]; } else { return 0; }
        }
    }
}
