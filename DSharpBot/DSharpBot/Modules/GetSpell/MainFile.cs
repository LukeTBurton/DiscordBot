using DSharpPlus.CommandsNext.Attributes;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using OfficeOpenXml;

namespace DSharpBot.Modules.GetSpell
{
    public class MainFile
    {
        [Command("getSpell")]
        [Description("This command will return the specified spells information")]
        public async Task getSpell(CommandContext Context, [Description("The spell you want to find")][RemainingText]string input)
        {
            await Context.TriggerTypingAsync();

            getDoc getDoc = new getDoc();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            InformationFile IR = new InformationFile();
            if (string.IsNullOrEmpty(IR.Name))
            {
                await getSpellExcel.BeginExcel(input, IR);

                if (!string.IsNullOrEmpty(IR.Name))
                {
                    builder = EmbedClass.startBuilder(IR, null);
                    //148212622112718850
                    builder
                        .WithAuthor($"{Context.User.Username} requested the spell {IR.Name}!")
                        .WithColor(DiscordColor.Purple)
                        .WithFooter($"Classes: {(string.IsNullOrEmpty(IR.Classes) ? "none found" : IR.Classes)}");
                }

            }
            if (string.IsNullOrEmpty(IR.Name))
            {
                string spells = await getSpells(input);

                string URI = "";


                URI = $"https://thebombzen.com/grimoire/spells/{spells}";
                HtmlDocument doc = getDoc.getHtmlDoc(URI).Result;

                if (doc.DocumentNode.SelectSingleNode("/html/body/div/div/div/article/table") != null)
                {
                    try
                    {
                        await Task.WhenAll(getSpellInfo.getName(doc, IR), getSpellInfo.getCastingTime(doc, IR), getSpellInfo.getComponents(doc, IR), getSpellInfo.getDuration(doc, IR), getSpellInfo.getRange(doc, IR), getSpellInfo.getSchoolSpellLevel(doc, IR), getSpellInfo.getDescritpion(doc, IR), getSpellInfo.getClasses(doc, IR), getSpellInfo.getTable(doc, IR));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                    }

                }
                else
                {
                    await Task.WhenAll(getSpellInfo.getName(doc, IR), getSpellInfo.getCastingTime(doc, IR), getSpellInfo.getComponents(doc, IR), getSpellInfo.getDuration(doc, IR), getSpellInfo.getRange(doc, IR), getSpellInfo.getSchoolSpellLevel(doc, IR), getSpellInfo.getDescritpion(doc, IR), getSpellInfo.getClasses(doc, IR));
                }


                if (!string.IsNullOrEmpty(IR.Name))
                {
                    builder = EmbedClass.startBuilder(IR, doc);

                    builder
                        .WithColor(DiscordColor.Purple)
                        .WithFooter($"Classes: {IR.Classes}");
                }

            }
            await Context.RespondAsync(null, false, builder.Build());

            getSpellInfo.conCat = string.Empty;
        }

        async static Task<string> getSpells(string spells)
        {
            if (spells != "Blindness/Deafness")
            {
                spells = spells.Replace(" ", "-").Replace("’", "").Replace("'", "").Replace("/", "-").ToLower();
            }
            else
            {
                spells = spells.Replace(" ", "-").Replace("’", "").Replace("'", "").Replace("/", "").ToLower();
            }

            return await Task.FromResult(spells);
        }

    }
}
