using System;
using System.Xml;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DSharpBot.Modules.getFeats
{
    class getFeatM
    {
        [Command("ListFeats")]
        [Description("Will list all feats it can find")]
        public async Task listFeats(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            string[] files = getFiles();

            List<string> tmp = new List<string>();

            foreach (var item in files)
            {
                 tmp.Add(Path.GetFileNameWithoutExtension(item));
            }

            StringBuilder builder = new StringBuilder();

            foreach (var item in tmp)
            {
                builder.Append($"{item}\n");
            }
            

            await ctx.RespondAsync(builder.ToString(),false ,null);
        }

        [Command("getFeat")]
        [Description("get the requested feat from a batch of xml files")]
        public async Task start(CommandContext ctx, [RemainingText][Description("input feat name")] string input)
        {
            await ctx.TriggerTypingAsync();

            Information info = new Information();
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            input = ti.ToTitleCase(input);


            string[] files = getFiles();

            IEnumerable<string> query = files.Where(x => x == $"{path}\\{input}.xml").Select(x => x);

            int amount = 0;

            if (query != null)
            {
                XDocument xDoc = XDocument.Load(query.First());

                getName(info, "//name", xDoc);//0
                getPrerequisites(info, "//prerequisites", xDoc);//1
                getSource(info, "//source", xDoc);//2
                getDesc(info, "//descriptions/desc", xDoc);//3
                getTraits(info, "//descriptions/trait", xDoc);//4+

                if (xDoc.XPathSelectElement("//optional") != null)
                {
                    getTblDesc(info, "//flaws/description", xDoc);
                    getTblHeaders(info, "//table/col1/col1header | //table/col2/col2header", xDoc);
                    getTblRows(info, "//table/col1 | //table/col2", xDoc);
                    getExtra(info, "//optional", xDoc);

                    createtable(info);

                    amount = 5 + info.Traits.Count() + info.TblHeaders.Count() + info.TblRows.Count + info.Extra.Count;
                }
                else
                {
                    amount = 4 + info.Traits.Count();
                }

                builder = embedBuilder.startBuilder(info, amount);


                builder
                    .WithAuthor($"{ctx.User.Username} requested the feat {info.Name}")
                    .WithColor(DiscordColor.Purple);

                await ctx.RespondAsync(null, false, builder.Build());
            }
        }
        private static void createtable(Information info)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < info.TblRows[0].Count; i++)
            {
                builder.AppendFormat("{0,-10}{1, 5}\n", info.TblRows[0][i], info.TblRows[1][i]);
            }

            info.table = builder.ToString();
        }
        private static void getName(Information info, string xPath, XDocument xDoc)
        {
            info.Name = xDoc.XPathSelectElement(xPath).Value;
        }
        private static void getPrerequisites(Information info, string xPath, XDocument xDoc)
        {
            info.Prerequisites = xDoc.XPathSelectElement(xPath).Value;
        }
        private static void getSource(Information info, string xPath, XDocument xDoc)
        {
            info.Source = xDoc.XPathSelectElement(xPath).Value;
        }
        private static void getDesc(Information info, string xPath, XDocument xDoc)
        {
            info.Desc = xDoc.XPathSelectElement(xPath).Value;
        }
        private static void getTraits(Information info, string xPath, XDocument xDoc)
        {
            var elementSto = xDoc.XPathSelectElements(xPath);
            List<string> tmp = new List<string>();
            foreach (var item in elementSto)
            {
                tmp.Add(item.Value);
            }

            info.Traits = tmp.ToArray();
        }
        private static void getTblDesc(Information info, string xPath, XDocument xDoc)
        {
            info.TblDescription = xDoc.XPathSelectElement(xPath).Value;
        }
        private static void getTblHeaders(Information info, string xPath, XDocument xDoc)
        {
            var elementSto = xDoc.XPathSelectElements(xPath);
            List<string> tmp = new List<string>();
            foreach (var item in elementSto)
            {
                tmp.Add(item.Value);
            }

            info.TblHeaders = tmp.ToArray();
        }
        private static void getTblRows(Information info, string xPath, XDocument xDoc)
        {
            var elementSto = xDoc.XPathSelectElements(xPath);
            List<List<string>> tmp = new List<List<string>>();
            foreach (var item in elementSto)
            {
                tmp.Add(new List<string>(item.Value.Split("#")));
            }

            info.TblRows = tmp;
        }
        private static void getExtra(Information info, string xPath, XDocument xDoc)
        {
            var elementSto = xDoc.XPathSelectElements(xPath);
            List<string> tmp = new List<string>();
            foreach (var item in elementSto)
            {
                tmp.AddRange(item.Value.Split("#"));

                tmp.RemoveAt(0);
            }

            info.Extra = tmp;
        }
        private static string path = @"C:\Users\lukeb\Google Drive\DBotFiles\Feats";

        public static string[] getFiles()
        {
            List<string> files = Directory.GetFiles(path).ToList();
            files.RemoveAt(files.IndexOf($@"{path}\Template.xml"));
            files.RemoveAt(files.IndexOf($@"{path}\desktop.ini"));

            return files.ToArray();
        }
    }
}
