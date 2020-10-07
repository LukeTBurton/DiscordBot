using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSharpBot.Modules
{
    [Group("List")]
    public class ListCommands
    {

        const string Path = @"C:\Users\lukeb\Google Drive\DBotFiles\toDoList.txt";

        [Command("Write")]
        [Description("Writes the input to a text file")]
        public async Task WriteTo(CommandContext ctx, [RemainingText]string Input)
        {
            using (StreamWriter NewFile = File.AppendText(Path)) //adds the TextFiles.Txt to the Path Variable
            {
                NewFile.WriteLine(Input);
                NewFile.Flush(); //exports all the strings into the file
                NewFile.Close(); // closes the opened file, so it can be used again for other applications
            }

            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            builder.WithColor(DiscordColor.Blue)
                .AddField("Add", Input, false);

            await ctx.RespondAsync("", false, builder.Build());
        }
        [Command("Read")]
        [Description("Reads the text file")]
        public async Task ReadFrom(CommandContext ctx)
        {
            string infoFromFile;
            using (StreamReader sr = new StreamReader(Path))
            {
                infoFromFile = await sr.ReadToEndAsync();
            }
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            builder.WithColor(DiscordColor.Blue)
                .AddField("ToDoList", infoFromFile, true);

            await ctx.RespondAsync("", false, builder.Build());

        }
        [Command("Remove")]
        [Description("Remove value from text file")]
        public async Task RemoveFrom(CommandContext ctx, [RemainingText]string Input)
        {
            List<string> newList = new List<string>();
            string[] text = File.ReadAllLines(Path);
            newList = text.ToList();
            if (newList.Contains(Input))
            {
                foreach (var item in text)
                {
                    if (item == Input)
                    {
                        newList.Remove(item);
                        File.WriteAllLines($"{Directory.GetCurrentDirectory()}\\toDoList.txt", newList);
                    }

                }
                await ctx.RespondAsync($"You removed {Input} from the To do list", false);
            }
            else
            {
                await ctx.RespondAsync($"{Input} doesn't exist in the To do list", false);
            }
        }
        [Command("dir")]
        [Description("gets the dir the file is stored in")]
        public async Task write(CommandContext ctx)
        {
            await ctx.RespondAsync(Directory.GetCurrentDirectory());
        }
    }
}
