using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class DeviceCommand : InteractionModuleBase
    {
        [SlashCommand("device", "Generates a basic Amino-Device-ID")]
        public async Task device()
        {
            await RespondAsync($"```\n{Amino.helpers.generate_device_id()}```");
        }
    }
}
