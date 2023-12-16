using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class amino_help : InteractionModuleBase
    {

        public static string helpText = $"List of Commands:\n" +
                $"**</amino-help:{Program.client.CurrentUser.Id}>** - Shows a Help / About menu\n" +
                $"**</device:{Program.client.CurrentUser.Id}>** - Generates a Device ID\n" +
                $"**</objectid:{Program.client.CurrentUser.Id}>** <URL> - Gets the Object ID of a specific Amino URL\n" +
                $"**</communityid:{Program.client.CurrentUser.Id}>** <communityURL> | <Amino URL from post, chat or user in community> - Gets the Community ID of a given Amino URL\n" +
                $"**</public-url:{Program.client.CurrentUser.Id}>** <user URL> - Gets the public profile URL of an Amino User from object ID or profile URL\n" +
                $"**</device-extra:{Program.client.CurrentUser.Id}>** <prefix> - Allows you to generate a Device ID with a specific Prefix\n" +
                $"**</verify:{Program.client.CurrentUser.Id}>** <deviceId> - Allows you to see if a device ID is valid or not\n" +
                $"**</web-device:{Program.client.CurrentUser.Id}>** - Allows you to generate a prefix 17 Device ID\n " +
                $"**</created-time:{Program.client.CurrentUser.Id}>** <user URL> - Allows you to see the Date and Time a user has joined Amino\n" +
                $"**</show-profile:{Program.client.CurrentUser.Id}>** <user URL> - Allows you to get information about someones public Profile";
        public static string aboutText = $"The AminoBot Project is a Discord Bot dedicated to being an Amino toolkit in the discord bot Format\n" +
                        $"The AminoBot Project is made in the C# Programming language and is powered by **Amino.NET**(*<https://github.com/FabioGaming/Amino.Net>*)\n" +
                        $"This bot has been developed and made possible by **@fabiogaming**(*1071594003717423134*)";


        [SlashCommand("amino-help", "See a list of commands and info about this bot!")]
        public async Task help()
        {
            EmbedBuilder response = new EmbedBuilder();
            response.Color = Color.Blue;
            response.Title = "AminoBot Help";
            response.Description = helpText;
            response.Footer = new EmbedFooterBuilder()
            {
                Text = "Thank you for using AminoBot"
            };
            response.AddField("Links", "[GitHub](https://github.com/FabioGaming/AminoBot)\n[Amino.NET Server](https://discord.com/invite/qyv8P2gegK)\n[Amino.NET GitHub](https://github.com/FabioGaming/Amino.Net)");
            


            var helpButton = new ButtonBuilder();
            helpButton.IsDisabled = true;
            helpButton.Style = ButtonStyle.Primary;
            helpButton.Label = "Help";
            helpButton.CustomId = "amino-help-help";
            
            var InfoButton = new ButtonBuilder();
            InfoButton.IsDisabled = false;
            InfoButton.Style = ButtonStyle.Secondary;
            InfoButton.Label = "About";
            InfoButton.CustomId = "amino-help-about";

            var msgcomponents = new ComponentBuilder().WithButton(helpButton).WithButton(InfoButton);

            await RespondAsync("", embeds: new[] { response.Build() }, components: msgcomponents.Build());
        }
    }
}
