using Amino;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Events
{
    public class InteractionHandler
    {
        public static async Task handleInteraction(SocketInteraction interaction)
        {
            try
            {
                

                if(interaction.Type != Discord.InteractionType.MessageComponent)
                {
                    if (!Utils.CoolDown.isCooldown(interaction.User.Id)) { await Utils.CoolDown.addUser(interaction.User.Id); } else { await interaction.RespondAsync("", new[] { Templates.Embeds.TimeOutEmbed(Utils.CoolDown.getTimeoutSeconds(interaction.User.Id)).Build() }, ephemeral: true); }
                }

                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
                var context = new SocketInteractionContext(Program.client, interaction);

                // Execute the incoming command.
                var result = await Program._interactionService.ExecuteCommandAsync(context, Program._services);
                
            }
            catch { }
        }
    }
}
