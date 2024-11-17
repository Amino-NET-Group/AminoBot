using Discord;
using Discord.Interactions;

namespace AminoBot
{
    [CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
    [IntegrationType(ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall)]
    public class Commands : InteractionModuleBase
    {

        private readonly Amino.Client _aminoClient;
        private readonly Utils _utils;

        public Commands(Amino.Client client, Utils utils)
        {
            _aminoClient = client;
            _utils = utils;
        }

        [SlashCommand("web-device", "Allows you to generate a prefix 17 device ID")]
        public async Task GetWebDevice()
        {
            await DeferAsync();
            try
            {

                if (_utils.GetWebDeviceIds().Count == 0) { await RespondAsync("", new[] { Templates.Embeds.noWebDevices().Build() }); }
                string randomDeviceId = _utils.GetWebDeviceIds()[new Random().Next(_utils.GetWebDeviceIds().Count)];
                await FollowupAsync("", new[] { Templates.Embeds.ResponseEmbed(randomDeviceId).Build() });
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.ResourceUnavailable().Build() }); }
        }

        [SlashCommand("verify", "Check if a device ID is valid or not")]
        public async Task checkDevice(string deviceId)
        {
            await DeferAsync();
            Amino.Client client = new Amino.Client();
            try
            {
                bool isValid = client.check_device(deviceId);
                await FollowupAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(deviceId, isValid.ToString()).Build() });
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("public-url", "Allows you to get someones public profile from community Profile URL or objectId")]
        public async Task GetPublicUser(string input)
        {
            await DeferAsync();
            try
            {

                Amino.Client client = Utils.mainClient;
                string _profileBase = input;
                if (input.StartsWith("http")) { _profileBase = client.get_from_code(input).objectId; }
                string profileBase = client.get_from_id(_profileBase, Amino.Types.Object_Types.User).shareURLShortCode;
                if (profileBase != null)
                {
                    await FollowupAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(input, profileBase).Build() });
                }
                else { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("show-profile", "Returns public user info in an Embed")]
        public async Task getProfile(string user)
        {
            await DeferAsync();
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
                string? _desc = AMProfile.content;
                if (_desc?.Length > 1024) { _desc = _desc.Substring(0, 1020); _desc = _desc + "..."; }
                if (_desc == null) { _desc = "*No Profile Description Available.*"; }
                profile.AddField("Profile Description", $"{_desc}");

                await FollowupAsync("", new[] { profile.Build() });
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("objectid", "Allows you to get an Object ID from an Amino URL")]
        public async Task getObjectId(string URL)
        {
            await DeferAsync();
            try
            {
                Amino.Client client = new Amino.Client();
                string objectId = client.get_from_code(URL).objectId;
                if (objectId != null)
                {
                    await FollowupAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(URL, objectId).Build() });
                }
                else
                {
                    await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() });
                }
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("device-extra", "Allows you to generate device IDs from a list of prefixes")]
        public async Task extraDeviceGen([Choice("18", 18), Choice("19", 19), Choice("22", 22), Choice("32", 32), Choice("42", 42), Choice("52", 52)] int prefix)
        {
            await RespondAsync("", new[] { Templates.Embeds.ResponseEmbed(Utils.ExtraGen.deviceId(prefix).ToUpper()).Build() });
        }

        [SlashCommand("device", "Generates a basic Amino-Device-ID")]
        public async Task device()
        {
            await RespondAsync("", new[] { Templates.Embeds.ResponseEmbed(Amino.helpers.generate_device_id()).Build() });
        }

        [SlashCommand("created-time", "Allows you to see when a user joined Amino")]
        public async Task createdTime(string userUrl)
        {
            await DeferAsync();
            try
            {
                Amino.Client client = new Amino.Client();
                string _user;
                if (userUrl.StartsWith("http")) { _user = client.get_from_code(userUrl).objectId; } else { _user = userUrl; }
                var user = client.get_user_info(_user);
                await FollowupAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(userUrl, user.createdTime!).Build() });
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("communityid", "Allows you to get the Community ID from a Community URL or post, chat or user URL within a community")]
        public async Task getCommunityId(string URL)
        {
            await DeferAsync();
            try
            {
                Amino.Client client = new Amino.Client();
                var _communityBase = client.get_from_code(URL);
                string? comId = null;
                if (_communityBase.Community != null) { comId = _communityBase.Community.communityId.ToString(); } else { comId = _communityBase.communityId.ToString(); }
                if (comId != null)
                {
                    await FollowupAsync("", new Discord.Embed[] { Templates.Embeds.ResponseTemplate(URL, comId).Build() });
                }
                else { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }

        [SlashCommand("show-community", "Allows you to see information about a community")]
        public async Task ShowCommunity(string communityUrl)
        {
            await DeferAsync();
            try
            {
                Amino.Client client = new Amino.Client();
                string _comId = communityUrl.StartsWith("http") ? client.get_from_code(communityUrl).communityId.ToString() : communityUrl; 

                var communityBase = client.get_community_info(_comId);

                EmbedBuilder community = new EmbedBuilder();
                community.Color = Color.Teal;
                if(communityBase.iconUrl != null) { community.ThumbnailUrl = communityBase.iconUrl; }
                community.Title = $"Community info of {communityBase.name}";


                string desc =
                    $"\nCommunityId: {communityBase.communityId}" +
                    $"\nActivity: {communityBase.communityHeat}" +
                    $"\nCreated Time: {communityBase.createdTime}" +
                    $"\nLast Modified: {communityBase.modifiedTime}" +
                    $"\nLast Updated: {communityBase.updatedTime}" +
                    $"\nStaff Count: {communityBase.communityHeadList.Count}" +
                    $"\nMember Count: {communityBase.membersCount}" +
                    $"\nJoin Type: {communityBase.joinType}" +
                    $"\nLink: {communityBase.link}" +
                    $"\nEndpoint: {communityBase.endpoint}" +
                    $"\nKeywords: {communityBase.keywords}" +
                    $"\nSearchable: {communityBase.searchable}" +
                    $"\nPrimary Language: {communityBase.primaryLanguage}" +
                    $"\nTagline: {communityBase.tagline}" +
                    $"\nIconUrl: [{communityBase.iconUrl?.Replace("http://", string.Empty)}]({communityBase.iconUrl})";

                community.Description = desc;
                
                if(communityBase.content != null)
                {
                    string _content = communityBase.content.Length > 1024 ? communityBase.content.Substring(0, 1020) + "..." : communityBase.content;
                    community.AddField("Description", _content);
                }
                if(communityBase.Agent != null)
                {
                    var agent = communityBase.Agent;
                    string _desc =
                        $"\nNickname: {agent.nickname}" +
                        $"\nUserId: {agent.userId}" +
                        $"\nFollower Count: {agent.membersCount}" +
                        $"\nReputation: {agent.reputation}" +
                        $"\nLevel: {agent.level}" +
                        $"\nIcon URL: [{agent.iconUrl.Replace("http://", string.Empty)}]({agent.iconUrl})";
                    community.AddField("Agent Info", _desc);
                }

                await FollowupAsync("", new[] { community.Build() });
                
            }
            catch { await FollowupAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
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
            response.AddField("Links", "[GitHub](https://github.com/Amino-NET-Group/AminoBot)\n[Amino.NET Server](https://discord.gg/2WjekrvArr)\n[Amino.NET GitHub](https://github.com/Amino-NET-Group/Amino.Net)");



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
