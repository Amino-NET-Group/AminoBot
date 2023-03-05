using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class CreatedTime : InteractionModuleBase
    {
        [SlashCommand("created-time", "Allows you to see when a user joined Amino")]
        public async Task createdTime(string userUrl)
        {
            try
            {
                Amino.Client client = new Amino.Client();
                string _user;
                if(userUrl.StartsWith("http")) { _user = client.get_from_code(userUrl).objectId; } else { _user = userUrl; }
                var user = client.get_user_info(_user);
                await RespondAsync($"```Base: {userUrl}\nResult: {user.createdTime}```");
            } catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }
    }
}
