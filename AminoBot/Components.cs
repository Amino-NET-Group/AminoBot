using Discord;
using Discord.Interactions;
using Discord.WebSocket;


namespace AminoBot
{
    public class Components : InteractionModuleBase
    {

        private readonly DiscordSocketClient _client;
        public Components(DiscordSocketClient client)
        {
            this._client = client;
        }

        [ComponentInteraction("amino-help_help")]
        public async Task HandleHelpCommandButton()
        {
            await DeferAsync();
            EmbedBuilder embed = new EmbedBuilder();
            ComponentBuilder components = new ComponentBuilder();

            embed.Color = Color.Teal;
            embed.Title = "AminoBot Help";
            embed.Description = "A list of all commands";

            foreach(var command in await _client.GetGlobalApplicationCommandsAsync())
            {
                embed.AddField($"</{command.Name}:{command.Id}>", command.Description);
            }

            components.WithButton("Help", "amino-help_help", ButtonStyle.Primary, disabled: true);
            components.WithButton("About", "amino-help_about", ButtonStyle.Secondary);

            await ModifyOriginalResponseAsync(msg => { msg.Embed = embed.Build(); msg.Components = components.Build(); });
        }

        [ComponentInteraction("amino-help_about")]
        public async Task HandleHelpAboutButton()
        {
            await DeferAsync();
            EmbedBuilder embed = new EmbedBuilder();
            ComponentBuilder components = new ComponentBuilder();
            embed.Title = "AminoBot Info";
            embed.Description = "AminoBot is a Discord Bot designed to act as a simple interface to interact with the Aminoapps API for Developers and Users alike.\nThis project is brought to you by the Amino.NET Group";
            embed.AddField("Links", "[Join our Discord Server!](https://discord.com/invite/2JeE54uG7x)\n[Github](https://github.com/Amino-NET-Group/AminoBot)\n[Amino.NET Github](https://github.com/Amino-NET-Group/Amino.NET)");

            components.WithButton("Help", "amino-help_help", ButtonStyle.Secondary);
            components.WithButton("About", "amino-help_about", ButtonStyle.Primary, disabled: true);

            await ModifyOriginalResponseAsync(msg => { msg.Embed = embed.Build(); msg.Components = components.Build(); });
        }
    }
}
