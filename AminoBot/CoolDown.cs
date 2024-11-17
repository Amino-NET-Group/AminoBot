using Discord;

namespace AminoBot
{
    public static class CoolDown
    {
        static Dictionary<ulong, long> timeoutList = new Dictionary<ulong, long>(); // userId : expirationTime


        /// <summary>
        /// Checks if the given user is on Cooldown, if the time expired it will automatically remove them from the list
        /// </summary>
        /// <param name="targetUser"></param>
        /// <returns></returns>
        public static bool IsOnCooldown(this IUser targetUser)
        {
            if (!timeoutList.ContainsKey(targetUser.Id)) return false;
            if (timeoutList[targetUser.Id] <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                timeoutList.Remove(targetUser.Id);
                return false;
            }
            return true;
        }


        public static Task AddUser(this IUser targetUser)
        {
            if(!timeoutList.ContainsKey(targetUser.Id))
            {
                timeoutList.Add(targetUser.Id, (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + new Utils().GetConfig().CommandTimeout));
            }
            return Task.CompletedTask;
        }


        public static long GetRemainingTimeoutSeconds(this IUser targetUser)
        {
            if (!timeoutList.ContainsKey(targetUser.Id)) return 0;
            return (timeoutList[targetUser.Id] - DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }

    }
}
