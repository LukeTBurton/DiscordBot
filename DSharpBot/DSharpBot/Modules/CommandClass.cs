using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
namespace DSharpBot
{
    public class CommandClass
    {
        [Command("hi")]
        [Description("Says Hi")]
        public async Task Hi(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivityModule();
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");

            var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id && xm.Content.ToLower() == "how are you?", TimeSpan.FromMinutes(1));
            if (msg != null)
                await ctx.RespondAsync($"I'm fine, thank you!");
            else
                await ctx.RespondAsync("rude");
        }
        [Command("ExcelLink")]
        [Description("sends the google drive link for the excel links")]
        public async Task getExcelLinkCommand(CommandContext ctx)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            builder
                .WithColor(DiscordColor.Purple)
                .AddField("Excel Link", @"https://drive.google.com/drive/folders/1x1RwLR-tdM8bhnWmNxucKb1jXtFhCX4l?usp=sharing");

            await ctx.RespondAsync("", false, builder.Build());
        }
    }
}
