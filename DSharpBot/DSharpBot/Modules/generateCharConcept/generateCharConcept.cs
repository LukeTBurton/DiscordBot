using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DSharpBot.Modules.generateCharConcept
{
    class generateCharConcept
    {
        [Command("Generate")]
        [Description("Will generate a random charector concept")]
        public async Task Start(CommandContext ctx)
        {
            var filePath = @"C:\Users\lukeb\Google Drive\DBotFiles\randomCharConcept.xlsx";
            FileInfo file = new FileInfo(filePath);

            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            builder
                .WithColor(DiscordColor.Blue)
                .AddField("This is your charactor. Enjoy", $"You are a {await getPersonalityASYNC(file)} {await getRaceASYNC(file)}, {await getClassASYNC(file)} from {await getLocationASYNC(file)} who {await getSomethingASYNC(file)}", false)
                .WithFooter("If you want you can add to the options, they are in an excel file somewhere");

            await ctx.RespondAsync("", false, builder.Build());

        }
        private async Task<string> getPersonalityASYNC(FileInfo file)
        {
            var Personality = "";

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                Random rng = new Random();

                int rngCell = rng.Next(0, sheet.Dimension.Rows);

                Personality = await Task.FromResult(sheet.Cells[rngCell, 1].Value.ToString());
            }


            return Personality;
        }
        private async Task<string> getRaceASYNC(FileInfo file)
        {
            var Race = "";

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];

                Random rng = new Random();

                int rngCell = rng.Next(0, sheet.Dimension.Rows);

                Race = await Task.FromResult(sheet.Cells[rngCell, 1].Value.ToString());
            }

            return Race;
        }
        private async Task<string> getClassASYNC(FileInfo file)
        {
            var Class = "";

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[2];

                Random rng = new Random();

                int rngCell = rng.Next(0, sheet.Dimension.Rows);

                Class = await Task.FromResult(sheet.Cells[rngCell, 1].Value.ToString());
            }

            return Class;
        }
        private async Task<string> getLocationASYNC(FileInfo file)
        {
            var Location = "";

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[3];

                Random rng = new Random();

                int rngCell = rng.Next(0, sheet.Dimension.Rows);

                Location = await Task.FromResult(sheet.Cells[rngCell, 1].Value.ToString());
            }

            return Location;
        }
        private async Task<string> getSomethingASYNC(FileInfo file)
        {
            var Something = "";

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[4];

                Random rng = new Random();

                int rngCell = rng.Next(0, sheet.Dimension.Rows);

                Something = await Task.FromResult(sheet.Cells[rngCell, 1].Value.ToString());
            }

            return Something;
        }
    }
}
