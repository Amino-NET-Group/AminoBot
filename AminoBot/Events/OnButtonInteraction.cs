using Discord;
using Discord.WebSocket;

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
                    response.Description = Utils.helpText;
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
                    response.Description = Utils.aboutText;
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
