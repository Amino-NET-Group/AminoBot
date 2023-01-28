using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class ObjectIdCommand : InteractionModuleBase
    {
        [SlashCommand("objectid", "Allows you to get an Object ID from an Amino URL")]
        public async Task getObjectId(string URL)
        {
            try
            {
                Amino.Client client = new Amino.Client();
                string objectId = client.get_from_code(URL).objectId;
                if (objectId != null)
                {
                    await RespondAsync($"```Base: {URL}\nResult: {objectId}```");
                }
                else
                {
                    await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() });
                }
            } catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }
    }
}
