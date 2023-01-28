using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class VerifyCommand : InteractionModuleBase
    {
        [SlashCommand("verify", "Check if a device ID is valid or not")]
        public async Task checkDevice(string deviceId)
        {
            Amino.Client client = new Amino.Client();
            try
            {
                bool isValid = client.check_device(deviceId);
                await RespondAsync($"```Base: {deviceId}\nIs Valid: {isValid}```");
            }catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }
    }
}
