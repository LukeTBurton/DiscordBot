using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DSharpBot.Modules.GetSpell
{
    class getSpellExcel
    {
        public static async Task<Task> BeginExcel(string input, InformationFile j)
        {
            var filePath = $@"C:\Users\lukeb\Google Drive\DBotFiles\homebrew.xlsx";
            FileInfo file = new FileInfo(filePath);

            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            if (input.Contains('’'))
            {
                int index = input.IndexOf("’");

                input = ti.ToTitleCase(input);

                input = input.Insert(index + 1, input[index + 1].ToString().ToLower());

                input = input.Remove((index + 2), 1);
            }
            else if (input.Contains("'"))
            {
                int index = input.IndexOf("'");

                input = ti.ToTitleCase(input);
                ti.ToLower(input[index + 1]);
            }
            else
            {
                input = ti.ToTitleCase(input);
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                for (int i = 1; i < sheet.Cells.Rows; i++)
                {
                    try
                    {
                        if (sheet.Cells[i, 1].Value.ToString().TrimStart().TrimEnd() == input)
                        {
                            j.Name = await getName(i, sheet);
                            j.Level = await getLevel(i, sheet);
                            j.DamageEffect = await getDmg(i, sheet);
                            j.Components = await getComp(i, sheet);
                            j.Duration = await getDura(i, sheet);
                            j.AttackSave = await getAtk(i, sheet);
                            j.CastingTime = await getCast(i, sheet);
                            j.RangeArea = await getRng(i, sheet);
                            j.School = await getSchool(i, sheet);
                            j.Description = await getDesc(i, sheet);

                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        return Task.CompletedTask;
                    }

                }

                return Task.CompletedTask;
            }
        }
        private static async Task<string> getName(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 1].Value.ToString());
        }
        private static async Task<string> getLevel(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 2].Value.ToString());
        }
        private static async Task<string> getDmg(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 3].Value.ToString());
        }
        private static async Task<string> getComp(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 4].Value.ToString());
        }
        private static async Task<string> getDura(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 5].Value.ToString());
        }
        private static async Task<string> getAtk(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 6].Value.ToString());
        }
        private static async Task<string> getCast(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 7].Value.ToString());
        }
        private static async Task<string> getRng(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 8].Value.ToString());
        }
        private static async Task<string> getSchool(int i, ExcelWorksheet sheet)
        {
            return await Task.FromResult(sheet.Cells[i, 9].Value.ToString());
        }
        private static async Task<string[]> getDesc(int i, ExcelWorksheet sheet)
        {
            string Desc = sheet.Cells[i, 10].Value.ToString();
            string[] desc = Desc.Split("\n\n");

            return await Task.FromResult(desc);
        }
    }
}
