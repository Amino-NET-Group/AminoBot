﻿using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Timers;

namespace AminoBot
{
    internal class Program
    {
        public static ServiceProvider _services;
        public static InteractionService _interactionService;
        public static DiscordSocketClient client;

        private static Amino.Client? _aminoClient = null;
        static async Task Main(string[] args)
        {
            Utils _utils = new Utils();

            Console.WriteLine("Starting Bot...");
            await _utils.CheckFiles();
            var config = _utils.GetConfig();
            if (config.BotToken == "") { Console.WriteLine("Please enter a bot token."); Console.ReadKey(); Environment.Exit(0); }
            _aminoClient = new Amino.Client(config.AminoAccountDeviceId);
            await _aminoClient.login(config.AminoAccountEmail, config.AminoAccountPassword, null, false);


            var socketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
                LogLevel = LogSeverity.Error,
                UseInteractionSnowflakeDate = false
            };

            client = new DiscordSocketClient(socketConfig);






            Console.WriteLine("Done!");
            Console.WriteLine("Logging in and starting");
            await client.LoginAsync(TokenType.Bot, config.BotToken);
            await client.StartAsync();
            _interactionService = new InteractionService(client.Rest);
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            Console.WriteLine("Bot started");


            //Events
            client.JoinedGuild += events.onServerJoin;
            client.LeftGuild += events.onServerLeave;
            client.Ready += async () =>
            {

                await _interactionService.RegisterCommandsGloballyAsync(true);
                Console.WriteLine("Commands Registered!");

                System.Timers.Timer timer = new();
                timer.Interval = 120000;
                timer.AutoReset = true;
                timer.Elapsed += UpdatePresence;
            };
            client.InteractionCreated += async (interaction) =>
            {
                // TODO
            };
            client.ButtonExecuted += events.onButtonPress;
            client.MessageReceived += events.onMsg;
            //Events end here


            Console.WriteLine("Ready");

            await Task.Delay(-1);

        }


        static async void UpdatePresence(object? sender, ElapsedEventArgs e)
        {
                try
                {
                    await client.SetGameAsync($"/amino-help on {client.Guilds.Count} servers", "", ActivityType.Listening);
                }
                catch { }
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

                Utils.InitText();

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