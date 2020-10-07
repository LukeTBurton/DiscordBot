using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using OfficeOpenXml;

namespace DSharpBot.Modules
{
    public class generatePoll
    {
        [Command("poll")]
        [Description("It creates a poll, the title of the poll is decided after the options have been selected")]
        public async Task executeCommand(CommandContext ctx, [Description("Every number needs to be followed by the correct unit, so 10s is 10 seconds, 10m is 10 minutes, 10h is 10 hours")] TimeSpan duration, [Description("WTF do u think it is")] params string[] Options)
        {
            string[] emojis = "👋 🤷 🖐 🧟 🖖 🧳 🌂 🧵 🧶 👓 🕶 🥽 🥼 🦺 👔 👕 👖 🧣 🧤 🧥 🧦 👗 👘 🥻 👙 👚 👛 👜 🦶 👂 🦻 👃 🧠 🦷 🦴 👀 👁 👅 👄 💋".Split(" ");

            var interactivity = ctx.Client.GetInteractivityModule();
            var options = Options.Select(x => x.ToString());

            await ctx.Channel.SendMessageAsync("input poll name").ConfigureAwait(false);

            var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromSeconds(5));

            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = (msg != null) ? msg.Message.Content : "No Title",
                Description = string.Join(" ", options)
            };

            var pollMessage = await ctx.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

            for (int i = 0; i < options.Count(); i++)
            {
                await pollMessage.CreateReactionAsync(DiscordEmoji.FromUnicode(emojis[i])).ConfigureAwait(false);
            }

            var result = await interactivity.CollectReactionsAsync(pollMessage, duration).ConfigureAwait(false);
            var distinctResult = result.Reactions.Distinct();

            var results = distinctResult.Select(x => $"{x.Value}: {x.Key}");

            await writeToExcel(results, msg.Message.Content);
        }


        private async Task writeToExcel(IEnumerable<string> results, string Title)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var filePath = $@"C:\Users\lukeb\Google Drive\DBotFiles\Polls.xlsx";
            FileInfo file = new FileInfo(filePath);

            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                bool test = false;
                int index = 0;

                string workSheetName = string.Empty;
                do
                {
                    try
                    {
                        if (index == 0)
                        {
                            package.Workbook.Worksheets.Add(Title);
                            workSheetName = Title;
                        }
                        else
                        {
                            package.Workbook.Worksheets.Add($"{Title} {index}");
                            workSheetName = $"{Title} {index}";
                        }
                        test = false;
                    }
                    catch
                    {
                        test = true;
                        index = index + 1;
                    }
                } while (test == true);



                ExcelWorksheet sheet = package.Workbook.Worksheets[workSheetName];

                for (int i = 0; i < results.Count(); i++)
                {
                    sheet.Cells[i + 1, 1].Value = results.ElementAt(i);
                }

                package.Save();
            }


        }

    }
}