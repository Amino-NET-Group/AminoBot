using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Templates
{
    public class Embeds
    {
        public static EmbedBuilder JoinedServer(SocketGuild guild)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "Joined Server";
            embed.Description = $"**Server Info**\nName: {guild.Name}\nID: {guild.Id}\nOwner ID: {guild.OwnerId}\nMembers: {guild.MemberCount}";
            embed.Author = new EmbedAuthorBuilder()
            {
                IconUrl = guild.IconUrl,
                Name = guild.Name
            };
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"{DateTime.Now.ToString("dd/MM/yyyy")} / {DateTime.Now.ToString("HH:mm:ss")}"
            };

            return embed;
        }

        public static EmbedBuilder LeftServer(SocketGuild guild)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "Left Server";
            embed.Description = $"**Server Info**\nName: {guild.Name}\nID: {guild.Id}";
            embed.Author = new EmbedAuthorBuilder()
            {
                IconUrl = guild.IconUrl,
                Name = guild.Name
            };
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"{DateTime.Now.ToString("dd/MM/yyyy")} / {DateTime.Now.ToString("HH:mm:ss")}"
            };

            return embed;
        }


        public static EmbedBuilder RequestError()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "Something went wrong!";
            embed.Description = $"Something went wrong while trying to execute this request!\nMake sure you gave correct data or try again later!";
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"Thank you for using AminoBot!"
            };

            return embed;
        }

        public static EmbedBuilder ResourceUnavailable()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "Something went wrong!";
            embed.Description = $"The resource you're trying to access might not be available at the moment!\nPlease try again later or report the issue.";
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"Thank you for using AminoBot!"
            };

            return embed;
        }

        public static EmbedBuilder TimeOutEmbed(int secondsLeft)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "Slow Down!";
            embed.Description = $"You are currently on timeout!\nYou must wait {secondsLeft} seconds before you can use the next Command!";
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"Thank you for using AminoBot!"
            };

            return embed;
        }

        public static EmbedBuilder webDeviceCountWarn(int currCount)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "AminoBot HUNGRY";
            embed.Description = $"The current amount of web Device IDs is less than 100 (Count: {currCount}), please refill web devices soon!";
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"{DateTime.Now.ToString("dd/MM/yyyy")} / {DateTime.Now.ToString("HH:mm:ss")}"
            };
            return embed;
        }

        public static EmbedBuilder webDevicesAdded(int count, int total)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "Added new Web Devices";
            embed.Description = $"Successfully added {count} web Device IDs. (New Total: {total})";
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"{DateTime.Now.ToString("dd/MM/yyyy")} / {DateTime.Now.ToString("HH:mm:ss")}"
            };
            return embed;
        }

        public static EmbedBuilder noWebDevices()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.Blue;
            embed.Title = "No Web Devices available";
            embed.Description = "It seems like there are no available web devices at the moment, try again later!";
            embed.Footer = new EmbedFooterBuilder()
            {
                Text = $"Thank you for using AminoBot!"
            };

            return embed;
        }

    }
}
