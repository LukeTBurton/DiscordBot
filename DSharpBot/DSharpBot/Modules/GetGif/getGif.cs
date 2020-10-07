using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DSharpBot.Modules
{
    public class getGif
    {
        [Command("getGif")]
        [Description("gets the specified gif from tenor")]
        public async Task getGifCommand(CommandContext ctx)
        {
            getDoc getDoc = new getDoc();
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            getGifUri(await getDoc.getHtmlDoc(filter(ctx.Message.Content)));

            builder
                .WithColor(DiscordColor.Blue)
                .WithImageUrl(getGifUri(await getDoc.getHtmlDoc(filter(ctx.Message.Content))));              
          
            await ctx.RespondAsync("", false, builder.Build());
        }

        static string filter(string query)
        {
            string baseUri = string.Empty;

            query = query.Replace(" ", "-");
            return $"https://tenor.com/search/{query}-gifs";
        }

        

        static string getGifUri(HtmlDocument doc)
        {
            HtmlNodeCollection nColl = doc.DocumentNode.SelectNodes("//*[img]");

            Random rnd = new Random();

            for (int i = rnd.Next(0, nColl.Count); i < nColl.Count; i++)
            {
                if (nColl[i].Name == "div")
                {
                    return nColl[i].LastChild.Attributes[0].Value;
                }
            }

            return null;
        }
    }
}
