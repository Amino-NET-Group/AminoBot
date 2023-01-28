using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Events
{
    public class OnServerLeave
    {
        public static async Task onServerLeave(SocketGuild guild)
        {
            try
            {
                ITextChannel channel = (ITextChannel)Program.client.GetChannel(Convert.ToUInt64(Utils.logChannel));
                await channel.SendMessageAsync("", false, Templates.Embeds.LeftServer(guild).Build());
            }
            catch { }
        }
    }
}
