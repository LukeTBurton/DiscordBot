using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpBot.Modules;
using DSharpBot.Modules.generateCharConcept;
using DSharpBot.Modules.getFeats;
using DSharpBot.Modules.GetSpell;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

//For Emoji use https://unicode.org/emoji/charts/full-emoji-list.html

namespace DSharpBot
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;
        static InteractivityModule interactivity;

        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {

                Token = "ADDYOUROWNTOKEN",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug              
               
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "*",
                EnableDms = true,
                CaseSensitive = false,
                EnableMentionPrefix = false
               
            });
            
            
            discord.MessageCreated += async e =>
            {

                if (e.Message.Content.ToLower().StartsWith("math") && !e.Author.IsBot)
                {
                    await commands.SudoAsync(e.Author, e.Channel, $"*math {e.Message.Content}");
                }

                var r = discord.Guilds;

                if (e.Message.Content.ToLower().StartsWith("pong"))
                {
                    await e.Message.RespondAsync("ping");
                }
                if (e.Message.Content.ToLower().StartsWith("usual time"))
                {
                    var info = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                    DateTimeOffset localServerTime = DateTimeOffset.Now;

                    foreach (var keys in discord.Guilds.Keys)
                    {                      
                        
                        if (keys == e.Guild.Id && keys == 729734575592570952)//Sails and Conquest D&D channel
                        {
                            if (TimeZoneInfo.Local.IsDaylightSavingTime(localServerTime))
                            {
                                await e.Message.RespondAsync($"@here The time has not been decided");
                            }
                            else
                            {
                                await e.Message.RespondAsync($"@here The time has not been decided");
                            }
                        }
                        else if (keys == e.Guild.Id && keys == 656242589166600213)//DnD the realms
                        {
                            if (TimeZoneInfo.Local.IsDaylightSavingTime(localServerTime))
                            {
                                await e.Message.RespondAsync($"@here the Time of the dnd is 19:30 BST and 20:30 BST+1, this might be subject to change, which will be specified by the host\nThe current time is, {localServerTime.AddHours(1).TimeOfDay} BST, {localServerTime.TimeOfDay} BST+1");
                            }
                            else
                            {
                                await e.Message.RespondAsync($"@here the Time of the dnd is 19:30 GMT and 20:30 GMT+1, this might be subject to change, which will be specified by the host\nThe current time is, {localServerTime.AddHours(1).TimeOfDay} GMT, {localServerTime.TimeOfDay} GMT+1");
                            }
                        }
                    }
                    

                }
                if (e.Message.Content.Contains("<@!646469656407834624>"))
                {
                    await e.Message.RespondAsync($"I have been summoned, what can i assit you with, use *help, to see what i can do :)");
                }
            };

            commands.CommandErrored += async e =>
            {
                Console.WriteLine("=====Errored=====");
                Console.WriteLine(DateTime.UtcNow);
                Console.WriteLine(e.Command);
                Console.WriteLine(e.Context);
                Console.WriteLine(e.Exception);
                Console.WriteLine("=====Errored=====");
            };

            commands.CommandExecuted += async e =>
            {
                Console.WriteLine("=====Executed=====");
                Console.WriteLine(DateTime.UtcNow);
                Console.WriteLine(e.Command);
                Console.WriteLine(e.Command.Aliases);
                Console.WriteLine(e.Command.Arguments);
                Console.WriteLine(e.Command.Description);
                Console.WriteLine(e.Context.Command);
                Console.WriteLine(e.Context.Guild);
                Console.WriteLine(e.Context.User);
                Console.WriteLine("=====Executed=====");
            };            

            discord.UseInteractivity(new InteractivityConfiguration{});

            //Between these two points is where the commands are registered
            commands.RegisterCommands<CommandClass>();
            commands.RegisterCommands<MainFile>();
            commands.RegisterCommands<ListCommands>();
            commands.RegisterCommands<getGif>();
            commands.RegisterCommands<generateCharConcept>();
            commands.RegisterCommands<getFeatM>();
            commands.RegisterCommands<MathCommand>();
            commands.RegisterCommands<generatePoll>();
            //End of command registration

            //Before connecting intialise stuffs
            await discord.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
