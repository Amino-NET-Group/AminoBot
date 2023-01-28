using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class PublicURLCommand : InteractionModuleBase
    {
        [SlashCommand("public-url", "Allows you to get someones public profile from community Profile URL or objectId")]
        public async Task GetPublicUser(string input)
        {
            try
            {

                Amino.Client client = Utils.mainClient;
                string _profileBase = input;
                if(input.StartsWith("http")) { _profileBase = client.get_from_code(input).objectId; }
                string profileBase = client.get_from_id(_profileBase, Amino.Types.Object_Types.User).shareURLShortCode;
                if (profileBase != null)
                {
                    await RespondAsync($"```Base: {input}\nResult: {profileBase}```");
                }
                else { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }
    }
}
