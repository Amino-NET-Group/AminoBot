using Discord;
using Discord.Interactions;

namespace AminoBot
{
    public class Commands : InteractionModuleBase
    {
        [SlashCommand("web-device", "Allows you to generate a prefix 17 device ID")]
        public async Task GetWebDevice()
        {
            try
            {
                Utils.ExtraGen.checkCount();
                if (Utils.webDevices.Count == 0) { await RespondAsync("", new[] { Templates.Embeds.noWebDevices().Build() }); }
                await RespondAsync($"```{Utils.ExtraGen.GetWebDevice()}```");
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.ResourceUnavailable().Build() }); }
        }

        [SlashCommand("verify", "Check if a device ID is valid or not")]
        public async Task checkDevice(string deviceId)
        {
            Amino.Client client = new Amino.Client();
            try
            {
                bool isValid = client.check_device(deviceId);
                await RespondAsync($"```Base: {deviceId}\nIs Valid: {isValid}```");
                await RespondAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(deviceId, isValid.ToString()).Build() });
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("public-url", "Allows you to get someones public profile from community Profile URL or objectId")]
        public async Task GetPublicUser(string input)
        {
            try
            {

                Amino.Client client = Utils.mainClient;
                string _profileBase = input;
                if (input.StartsWith("http")) { _profileBase = client.get_from_code(input).objectId; }
                string profileBase = client.get_from_id(_profileBase, Amino.Types.Object_Types.User).shareURLShortCode;
                if (profileBase != null)
                {
                    await RespondAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(input, profileBase).Build() });
                }
                else { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("show-profile", "Returns public user info in an Embed")]
        public async Task getProfile(string user)
        {
            try
            {
                string profileBase = user;

                if (user.StartsWith("http")) { profileBase = Utils.mainClient.get_from_code(user).objectId; }
                var AMProfile = Utils.mainClient.get_user_info(profileBase);
                var contentBase = Utils.mainClient.get_from_id(profileBase, Amino.Types.Object_Types.User);

                EmbedBuilder profile = new EmbedBuilder();
                profile.Color = Color.Teal;
                if (AMProfile.iconUrl != null) { profile.ThumbnailUrl = AMProfile.iconUrl; }

                profile.Title = $"Profile Info of: {AMProfile.nickname}";
                string desc =
                    $"\nAminoId: @{AMProfile.aminoId}" +
                    $"\nUserId: {AMProfile.userId}" +
                    $"\nUsername: {AMProfile.nickname}" +
                    $"\nFollower Count: {AMProfile.memberCount}" +
                    $"\nFollowing Count: {AMProfile.joinedCount}" +
                    $"\nWallcomments Count: {AMProfile.commentsCount}" +
                    $"\nPublicUrl: [{contentBase.shareURLShortCode.Replace("http://", String.Empty)}]({contentBase.shareURLShortCode})" +
                    $"\nAccount Created at: {AMProfile.createdTime}" +
                    $"\nAccount Last Modified at: {AMProfile.modifiedTime}";

                if (AMProfile.iconUrl != null) { desc = desc + $"\nIconUrl: [{AMProfile.iconUrl.Replace("http://", String.Empty)}]({AMProfile.iconUrl})"; }
                profile.AddField("Overview", desc);
                //profile.Description = desc;
                string _desc = AMProfile.content;
                if (_desc.Length > 1024) { _desc = _desc.Substring(0, 1020); _desc = _desc + "..."; }
                if (_desc == null) { _desc = "*No Profile Description Available.*"; }
                profile.AddField("Profile Description", $"{_desc}");

                await RespondAsync("", new[] { profile.Build() });
            }
            catch (Exception e) { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("objectid", "Allows you to get an Object ID from an Amino URL")]
        public async Task getObjectId(string URL)
        {
            try
            {
                Amino.Client client = new Amino.Client();
                string objectId = client.get_from_code(URL).objectId;
                if (objectId != null)
                {
                    await RespondAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(URL, objectId).Build() });
                }
                else
                {
                    await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() });
                }
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("device-extra", "Allows you to generate device IDs from a list of prefixes")]
        public async Task extraDeviceGen([Choice("18", 18), Choice("19", 19), Choice("22", 22), Choice("32", 32), Choice("42", 42), Choice("52", 52)] int prefix)
        {
            Utils.CoolDown.removeUser(Context.User.Id);
            await RespondAsync($"```\n{Utils.ExtraGen.deviceId(prefix).ToUpper()}```");
        }

        [SlashCommand("device", "Generates a basic Amino-Device-ID")]
        public async Task device()
        {
            Utils.CoolDown.removeUser(Context.User.Id);
            await RespondAsync($"```\n{Amino.helpers.generate_device_id()}```");
        }

        [SlashCommand("created-time", "Allows you to see when a user joined Amino")]
        public async Task createdTime(string userUrl)
        {
            try
            {
                Amino.Client client = new Amino.Client();
                string _user;
                if (userUrl.StartsWith("http")) { _user = client.get_from_code(userUrl).objectId; } else { _user = userUrl; }
                var user = client.get_user_info(_user);
                await RespondAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(userUrl, user.createdTime).Build() });
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("communityid", "Allows you to get the Community ID from a Community URL or post, chat or user URL within a community")]
        public async Task getCommunityId(string URL)
        {
            try
            {
                Amino.Client client = new Amino.Client();
                var _communityBase = client.get_from_code(URL);
                string comId = null;
                if (_communityBase.Community != null) { comId = _communityBase.Community.communityId.ToString(); } else { comId = _communityBase.communityId.ToString(); }
                if (comId != null)
                {
                    await RespondAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(URL, comId).Build() });
                }
                else { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
            }
            catch { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("amino-help", "See a list of commands and info about this bot!")]
        public async Task help()
        {
            EmbedBuilder response = new EmbedBuilder();
            response.Color = Color.Blue;
            response.Title = "AminoBot Help";
            response.Description = Utils.helpText;
            response.Footer = new EmbedFooterBuilder()
            {
                Text = "Thank you for using AminoBot"
            };
            response.AddField("Links", "[GitHub](https://github.com/FabioGaming/AminoBot)\n[Amino.NET Server](https://discord.com/invite/qyv8P2gegK)\n[Amino.NET GitHub](https://github.com/FabioGaming/Amino.Net)");



            var helpButton = new ButtonBuilder();
            helpButton.IsDisabled = true;
            helpButton.Style = ButtonStyle.Primary;
            helpButton.Label = "Help";
            helpButton.CustomId = "amino-help-help";

            var InfoButton = new ButtonBuilder();
            InfoButton.IsDisabled = false;
            InfoButton.Style = ButtonStyle.Secondary;
            InfoButton.Label = "About";
            InfoButton.CustomId = "amino-help-about";


            var msgcomponents = new ComponentBuilder().WithButton(helpButton).WithButton(InfoButton);

            await RespondAsync("", embeds: new[] { response.Build() }, components: msgcomponents.Build());
        }

    }
}
