using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AminoBot.Objects
{
    public class Config
    {
        [JsonPropertyName("bot_token")] public string BotToken { get; set; } = "";
        [JsonPropertyName("log_webhook_url")] public string LogWebhookUrl { get; set; } = "";
        [JsonPropertyName("amino_account_email")] public string AminoAccountEmail { get; set; } = "";
        [JsonPropertyName("amino_account_password")] public string AminoAccountPassword { get; set; } = "";
        [JsonPropertyName("amino_account_device_id")] public string AminoAccountDeviceId { get; set; } = "";
    }
}
