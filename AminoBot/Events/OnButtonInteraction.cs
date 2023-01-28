using AminoBot.Commands;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Events
{
    public class OnButtonInteraction
    {
        public static async Task onButtonInteraction(SocketMessageComponent button)
        {
            EmbedBuilder response = new EmbedBuilder();
            var helpButton = new ButtonBuilder();
            var InfoButton = new ButtonBuilder();
            var msgcomponents = new ComponentBuilder();


            switch (button.Data.CustomId)
            {
                case "amino-help-help":
                    
                    response.Color = Color.Blue;
                    response.Title = "AminoBot Help";
                    response.Description = $"List of Commands:\n" +
                        $"**/amino-help** - Shows a Help / About menu\n" +
                        $"**/device** - Generates a Device ID\n" +
                        $"**/objectid** <URL> - Gets the Object ID of a specific Amino URL\n" +
                        $"**/communityid** <communityURL> | <Amino URL from post, chat or user in community> - Gets the Community ID of a given Amino URL\n" +
                        $"**/public-url** <user URL> - Gets the public profile URL of an Amino User\n" +
                        $"**/device-extra** <prefix> - Allows you to generate a Device ID with a specific Prefix";
                    response.Footer = new EmbedFooterBuilder()
                    {
                        Text = "Thank you for using AminoBot"
                    };

                    helpButton = new ButtonBuilder();
                    helpButton.IsDisabled = true;
                    helpButton.Style = ButtonStyle.Primary;
                    helpButton.Label = "Help";
                    helpButton.CustomId = "amino-help-help";

                    InfoButton = new ButtonBuilder();
                    InfoButton.IsDisabled = false;
                    InfoButton.Style = ButtonStyle.Secondary;
                    InfoButton.Label = "About";
                    InfoButton.CustomId = "amino-help-about";

                    msgcomponents = new ComponentBuilder().WithButton(helpButton).WithButton(InfoButton);
                    break;
                case "amino-help-about":
                    response = new EmbedBuilder();
                    response.Color = Color.Blue;
                    response.Title = "AminoBot About";
                    response.Description = amino_help.aboutText;
                    response.Footer = new EmbedFooterBuilder()
                    {
                        Text = "Thank you for using AminoBot"
                    };

                    helpButton = new ButtonBuilder();
                    helpButton.IsDisabled = false;
                    helpButton.Style = ButtonStyle.Secondary;
                    helpButton.Label = "Help";
                    helpButton.CustomId = "amino-help-help";

                    InfoButton = new ButtonBuilder();
                    InfoButton.IsDisabled = true;
                    InfoButton.Style = ButtonStyle.Primary;
                    InfoButton.Label = "About";
                    InfoButton.CustomId = "amino-help-about";

                    msgcomponents = new ComponentBuilder().WithButton(helpButton).WithButton(InfoButton);
                    break;
            }
            await button.UpdateAsync(msg => { msg.Embed = response.Build(); msg.Components = msgcomponents.Build(); });
        }

    }
}
