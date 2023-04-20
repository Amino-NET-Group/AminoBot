using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class ExtraDeviceGenCommand : InteractionModuleBase
    {
        [SlashCommand("device-extra", "Allows you to generate device IDs from a list of prefixes")]
        public async Task extraDeviceGen([Choice("18", 18), Choice("19", 19), Choice("22", 22), Choice("32", 32), Choice("42", 42), Choice("52", 52)] int prefix)
        {
            Utils.CoolDown.removeUser(Context.User.Id);
            await RespondAsync($"```\n{Utils.ExtraGen.deviceId(prefix).ToUpper()}```");
        }
    }
}
