using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoBot.Commands
{
    public class ProfileInfoCommand : InteractionModuleBase
    {
        [SlashCommand("show-profile", "Returns public user info in an Embed")]
        public async Task getProfile(string user)
        {
            try
            {
                string profileBase = user;
                
                if(user.StartsWith("http")) { profileBase = Utils.mainClient.get_from_code(user).objectId; }
                var AMProfile = Utils.mainClient.get_user_info(profileBase);
                var contentBase = Utils.mainClient.get_from_id(profileBase, Amino.Types.Object_Types.User);

                EmbedBuilder profile = new EmbedBuilder();
                profile.Color = Color.Teal;
                if(AMProfile.iconUrl != null) { profile.ThumbnailUrl = AMProfile.iconUrl; }
                
                profile.Title = $"Profile Info of: {AMProfile.nickname}";
                string desc = $"**Overview**" +
                    $"\nAminoId: @{AMProfile.aminoId}" +
                    $"\nUserId: {AMProfile.userId}" +
                    $"\nUsername: {AMProfile.nickname}" +
                    $"\nFollower Count: {AMProfile.memberCount}" +
                    $"\nFollowing Count: {AMProfile.joinedCount}" +
                    $"\nWallcomments Count: {AMProfile.commentsCount}" +
                    $"\nPublicUrl: [{contentBase.shareURLShortCode}]({contentBase.shareURLShortCode})" +
                    $"\nAccount Created at: {AMProfile.createdTime}" +
                    $"\nAccount Last Modified at: {AMProfile.modifiedTime}";
                if(AMProfile.iconUrl != null) { desc = desc + $"\nIconUrl: [{AMProfile.iconUrl}]({AMProfile.iconUrl})"; }
                profile.Description = desc;
                string _desc = AMProfile.content;
                if(_desc.Length > 1024) { _desc = _desc.Substring(0, 1020); _desc = _desc + "..."; }
                if(_desc == null) { _desc = "*No Profile Description Available.*"; }
                profile.AddField("Profile Description", $"{_desc}");

                await RespondAsync("", new[] { profile.Build() });



            } catch (Exception e) { await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }
    }
}
