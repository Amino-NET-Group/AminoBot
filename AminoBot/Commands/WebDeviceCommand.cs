using Discord.Interactions;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class WebDeviceCommand : InteractionModuleBase
    {
        [SlashCommand("web-device", "Allows you to generate a prefix 17 device ID")]
        public async Task GetWebDevice()
        {
            try
            {
                Utils.ExtraGen.checkCount();
                if(Utils.webDevices.Count == 0) { await RespondAsync("", new[] { Templates.Embeds.noWebDevices().Build() }); }
                await RespondAsync($"```{Utils.ExtraGen.GetWebDevice()}```");
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.ResourceUnavailable().Build() }); }



        }
    }
}
