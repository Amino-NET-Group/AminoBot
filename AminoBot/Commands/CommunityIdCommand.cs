using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AminoBot.Commands
{
    public class CommunityIdCommand : InteractionModuleBase
    {
        [SlashCommand("communityid", "Allows you to get the Community ID from a Community URL or post, chat or user URL within a community")]
        public async Task getCommunityId(string URL)
        {
            try
            {
                Amino.Client client = new Amino.Client();
                var _communityBase = client.get_from_code(URL);
                string comId = null;
                if(_communityBase.Community != null) { comId = _communityBase.Community.communityId.ToString(); } else { comId = _communityBase.communityId.ToString(); }
                if(comId != null)
                {
                    await RespondAsync($"```Base: {URL}\nResult: {comId}```");
                } else { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
            }catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }
    }
}
