using AminoBot.Events;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.CSharp;
using Microsoft.Extensions.DependencyInjection;
using System.CodeDom.Compiler;
using System.Reflection;

namespace AminoBot
{
    internal class Program
    {
        public static ServiceProvider _services;
        public static InteractionService _interactionService;
        public static DiscordSocketClient client;
        

        static async Task Main(string[] args)
        {


            Console.WriteLine("Starting Bot...");
            Utils.CreateConfig();
            Utils.ReadConfig();
            if (Utils.botToken == "") { Console.WriteLine("Please enter a bot token."); Console.ReadKey(); Environment.Exit(0); }
            Utils.mainClient = new Amino.Client(Utils.clientDevice);
            await Utils.mainClient.login(Utils.clientEmail, Utils.clientPassword);
 

            var token = Utils.botToken;
            Events events = new Events();


            var socketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
                LogLevel = LogSeverity.Error,
                UseInteractionSnowflakeDate = false
            };

            client = new DiscordSocketClient(socketConfig);






            Console.WriteLine("Done!");
            Console.WriteLine("Logging in and starting");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            _interactionService = new InteractionService(client.Rest);
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            Console.WriteLine("Bot started");


            //Events
            client.JoinedGuild += events.onServerJoin;
            client.LeftGuild += events.onServerLeave;
            client.Ready += events.onClientReady;
            client.InteractionCreated += events.onInteraction;
            client.ButtonExecuted += events.onButtonPress;
            client.MessageReceived += events.onMsg;
            //Events end here


            Console.WriteLine("Ready");
            await Task.Delay(-1);

        }


        public static async Task updatePresence()
        {
            while(true)
            {
                try
                {
                    await client.SetGameAsync($"/amino-help on {client.Guilds.Count} servers", "", ActivityType.Listening);
                }
                catch { }
                await Task.Delay(25000);
            }
        }


        class Events
        {


            public async Task onServerJoin(SocketGuild guild)
            {
                await AminoBot.Events.OnServerJoin.onServerJoin(guild);
            }

            public async Task onServerLeave(SocketGuild guild)
            {
                await AminoBot.Events.OnServerLeave.onServerLeave(guild);
            }
            public async Task onClientReady()
            {
                await _interactionService.RegisterCommandsGloballyAsync(true);
                Console.WriteLine("Commands Registered!");

                _ = Task.Run(async () => updatePresence());
            }
            public async Task onInteraction(SocketInteraction interaction)
            {
                await AminoBot.Events.InteractionHandler.handleInteraction(interaction);
            }
            public async Task onButtonPress(SocketMessageComponent button)
            {
                await AminoBot.Events.OnButtonInteraction.onButtonInteraction(button);
            }
            public async Task onMsg(SocketMessage msg)
            {
                Utils.deviceFileCheck(msg);
            }

        }
    }
}