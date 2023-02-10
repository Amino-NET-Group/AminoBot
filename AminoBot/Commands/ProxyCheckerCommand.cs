using Amino;
using Discord.Interactions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using Discord;

namespace AminoBot.Commands
{
   public class ProxyCheckerCommand : InteractionModuleBase
   {
        //MIGHT BE REMADE LATER ON
        //[SlashCommand("checkproxy", "Allows you to see if a proxy is valid for Amino or not")]
        public async Task checkProxy(string proxy, [Choice("SOCKS5", "sock5"), Choice("SOCKS4", "sock4"), Choice("HTTPS", "https"), Choice("HTTP", "http")] string proxyType)
        {
            try
            {
                ProxyClient proxyClient = new ProxyClient();
                EmbedBuilder proxyCheckMsg = new EmbedBuilder();
                await RespondAsync($"```" +
                    $"Proxy: {proxy}" +
                    $"\nType: {proxyType}" +
                    $"\nIs Valid: {isValidProxy(proxy, proxyType).ToString()}" +
                    $"\nIs Valid for Amino: {proxyClient.check_device(proxy, proxyType).ToString()}" +
                    $"```");

            }
            catch (Exception e) { Console.WriteLine(e); await RespondAsync("", new[] { Templates.Embeds.RequestError().Build() }); }
        }


        private bool isValidProxy(string proxy, string proxyType)
        {
            try
            {
                RestClient client = new RestClient("https://google.com");
                RestRequest request = new RestRequest();
                client.Options.Proxy = new WebProxy($"{proxyType}://{proxy}");
                client.Options.MaxTimeout = 1500;
                var response = client.ExecuteGet(request);
                if((int)response.StatusCode != 200) { return false; }
                return true;

            } catch { return false; }
        }

        private class ProxyClient
        {
            private IDictionary<string, string> headers = new Dictionary<string, string>();

            //Handles the header stuff
            private Task headerBuilder()
            {
                headers.Clear();
                headers.Add("NDCDEVICEID", Amino.helpers.generate_device_id());
                headers.Add("Accept-Language", "en-US");
                headers.Add("Content-Type", "application/json; charset=utf-8");
                headers.Add("Host", "service.aminoapps.com");
                headers.Add("Accept-Encoding", "gzip");
                headers.Add("Connection", "Keep-Alive");
                headers.Add("User-Agent", "Apple iPhone13,4 iOS v15.6.1 Main/3.12.9");
                return Task.CompletedTask;
            }


            public ProxyClient()
            {
                headerBuilder();
            }
            public bool check_device(string proxy, string proxyType)
            {
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

                JObject data = new JObject();
                data.Add("deviceID", Amino.helpers.generate_device_id());
                data.Add("bundleID", "com.narvii.amino.master");
                data.Add("clientType", 100);
                data.Add("systemPushEnabled", true);
                data.Add("timezone", 0);
                data.Add("locale", currentCulture.Name);
                data.Add("timestamp", helpers.GetTimestamp() * 1000);

                try
                {
                    RestClient client = new RestClient(helpers.BaseUrl);
                    RestRequest request = new RestRequest("/g/s/device");
                    client.Options.MaxTimeout = 1500;
                    client.Options.Proxy = new WebProxy($"{proxyType}://{proxy}");
                    request.AddJsonBody(JsonConvert.SerializeObject(data));
                    request.AddHeaders(headers);
                    request.AddHeader("NDC-MSG-SIG", helpers.generate_signiture(JsonConvert.SerializeObject(data)));
                    var response = client.ExecutePost(request);
                    if ((int)response.StatusCode != 200) { return false; }
                    return true;
                }
                catch (Exception e) { throw new Exception(e.Message); }
            }

        }

    }
}
