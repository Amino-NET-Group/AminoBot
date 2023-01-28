using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Events
{
    public class OnServerJoin
    {
        public static async Task onServerJoin(SocketGuild guild)
        {
            EmbedBuilder welcomeEmbed = new EmbedBuilder();
            welcomeEmbed.Color = Color.Blue;
            welcomeEmbed.Title = "Thank you for Inviting me!";
            welcomeEmbed.Description = $"Thank you for inviting me to **{guild.Name}**!\nFor help you can view my Descrption or type /amino-help";
            welcomeEmbed.Footer = new EmbedFooterBuilder()
            {
                Text = "Thank you for supporting this Project!"
            };
            try
            {
                //await guild.Owner.SendMessageAsync("", false, welcomeEmbed.Build());
            }
            catch { }
            try
            {
                await guild.SystemChannel.SendMessageAsync("", false, welcomeEmbed.Build());
            }
            catch { }
            try
            {
                ITextChannel channel = (ITextChannel)Program.client.GetChannel(Convert.ToUInt64(Utils.logChannel));
                await channel.SendMessageAsync("", false, Templates.Embeds.JoinedServer(guild).Build());
            }
            catch { }

        }
    }
}
