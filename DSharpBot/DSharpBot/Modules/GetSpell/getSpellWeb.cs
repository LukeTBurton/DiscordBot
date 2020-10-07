using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpBot;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Linq;
namespace DSharpBot.Modules.GetSpell
{      

    public static class getSpellInfo
    {
        public async static Task<Task> getName(HtmlDocument doc, InformationFile IR)
        {
            string xPath = "/html/body/div/div/div/header/h1";

            IR.Name = doc.DocumentNode.SelectSingleNode(xPath).InnerText;

            return Task.CompletedTask;
        }
        public async static Task<Task> getSchoolSpellLevel(HtmlDocument doc, InformationFile IR)
        {
            string xPath = "/html/body/div/div/div/article/p[1]/strong";

            string[] schools =
            {
                "Evocation",
                "Abjuration",
                "Conjuration",
                "Divination",
                "Enchantment",
                "Illusion",
                "Necromancy",
                "Transmutation"
            };

            string[] spellLevels =
            {
                "Cantrip",
                "1st-Level",
                "2nd-Level",
                "3rd-Level",
                "4th-Level",
                "5th-Level",
                "6th-Level",
                "7th-Level",
                "8th-Level",
                "9th-Level"
            };

            //Actual Area thats important//

            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);

            int schoolIndex = 0;
            int spellIndex = 0;

            for (int i = 0; i < schools.Length; i++)
            {
                if (node.InnerText.Contains(schools[i].ToLower()))
                {
                    schoolIndex = i;
                }
            }
            for (int i = 0; i < spellLevels.Length; i++)
            {
                if (node.InnerText.Contains(spellLevels[i].ToLower()))
                {
                    spellIndex = i;
                }
            }

            IR.School = schools[schoolIndex];
            IR.Level = spellLevels[spellIndex];

            return Task.CompletedTask;
        }
        public async static Task<Task> getCastingTime(HtmlDocument doc, InformationFile IR)
        {
            string xPath = "/html/body/div/div/div/article/p[2]";

            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);

            IR.CastingTime = node.InnerText.Substring("Casting Time: ".Length, (node.InnerText.Length - "Casting Time: ".Length));

            return Task.CompletedTask;
        }

        public static string conCat = "";
        static void Store(string s)
        {
            conCat += @$"
{s}";

        }
        internal async static Task getTable(HtmlDocument doc, InformationFile IR)
        {
            string xPathA = "/html/body/div/div/div/article/table/thead";
            string xPathB = "/html/body/div/div/div/article/table/tbody";

            HtmlNode tableTitles = doc.DocumentNode.SelectSingleNode(xPathA);

            string Titles = getTTitles(tableTitles).Result;//That is table titles done

            //getting tablebody info
            HtmlNode tableBody = doc.DocumentNode.SelectSingleNode(xPathB);

            List<string> tableBodyResults = getTableBody(tableBody, doc, Titles).Result;

            tableBodyResults.ForEach(Store);

            IR.tableContents = conCat;

        }


        internal static Task<List<string>> getTableBody(HtmlNode node, HtmlDocument doc, string titles)
        {
            HtmlNodeCollection tableBodyChild = node.ChildNodes;

            List<HtmlNode> filteredChild = new List<HtmlNode>();

            List<string> splitChild = new List<string>();
            for (int i = 0; i < tableBodyChild.Count; i++)
            {
                if (tableBodyChild[i].InnerText == "" || tableBodyChild[i].InnerText == "\n")
                {

                }
                else
                {
                    filteredChild.Add(tableBodyChild[i]);
                }
            }

            //split

            char[] sep = { '\n' };
            splitChild.Add(titles);
            for (int i = 0; i < filteredChild.Count; i++)
            {
                splitChild.Add(filteredChild[i].InnerText.Replace(sep[0], ' '));
            }
            for (int i = 0; i < splitChild.Count; i++)
            {
                if (Regex.IsMatch(splitChild[i], @"[^\u0000-\u007F]+"))
                {
                    splitChild[i] = Regex.Replace(splitChild[i], @"[^\u0000-\u007F]+", "-");
                }
            }


            return Task.FromResult(splitChild);
        }

        internal static Task<string> getTTitles(HtmlNode node)
        {
            char[] sep = { '\n' };
            string[] o = node.InnerText.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            string titles = string.Join(" ", o);

            return Task.FromResult(titles);
        }

        public async static Task<Task> getRange(HtmlDocument doc, InformationFile IR)
        {
            string xPath = "/html/body/div/div/div/article/p[3]";

            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);

            IR.RangeArea = node.InnerText.Substring("Range: ".Length, (node.InnerText.Length - "Range: ".Length));

            return Task.CompletedTask;

        }
        public async static Task<Task> getComponents(HtmlDocument doc, InformationFile IR)
        {
            string xPath = "/html/body/div/div/div/article/p[4]";

            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);

            IR.Components = node.InnerText.Substring("Components: ".Length, (node.InnerText.Length - "Components: ".Length));

            return Task.CompletedTask;
        }
        public async static Task<Task> getDuration(HtmlDocument doc, InformationFile IR)
        {
            string xPath = "/html/body/div/div/div/article/p[5]";

            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);

            IR.Duration = node.InnerText.Substring("Duration: ".Length, (node.InnerText.Length - "Duration: ".Length));

            return Task.CompletedTask;
        }
        public async static Task<Task> getDescritpion(HtmlDocument doc, InformationFile IR)
        {
            
            string xPath = "//*[text()[string-length() > 80]]";

            if (IR.Name == "Enhance Ability")
            {
                xPath = "//p[text()[string-length() >= 43]]";
            }

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xPath);

            List<string> listNodes = new List<string>();

            foreach (var item in nodes)
            {
                if (!item.InnerText.StartsWith("\n"))
                {
                    listNodes.Add(item.InnerText);
                }
            }

            if (IR.Name == "Enhance Ability")
            {
                listNodes.RemoveAt(listNodes.Count-1);
            }

            IR.Description = listNodes.ToArray();

            //start getAttackSave Method//

            await getAttackSave(IR, listNodes);

            await getDmgType(listNodes, IR);

            return Task.CompletedTask;
        }
        public async static Task<Task> getClasses(HtmlDocument doc, InformationFile IR)
        {
            string[] classes =
            {
                "druid",
                "cleric",
                "bard",
                "paladin",
                "ranger",
                "sorcerer",
                "warlock",
                "wizard"
            };

            //start of actual method//

            string xPath = "/html/body/div/div/div/header/dl/dl";
            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);

            List<string> tags = new List<string>();
            tags.AddRange(node.InnerText.Split("\n\n"));

            List<string> classesFound = new List<string>();

            for (int i = 0; i < classes.Length; i++)
            {
                classesFound.Add(tags.Find(x => x.Contains(classes[i])));
                if (classesFound[i] != null)
                {
                    if (classesFound[i].Contains("\n"))
                    {
                        classesFound[i] = Regex.Replace(classesFound[i], @"\n", "");
                    }
                }
            }
            classesFound.RemoveAll(x => x == null);

            IR.Classes = string.Join(",", classesFound.ToArray());

            return Task.CompletedTask;
        }
        internal static Task getDmgType(List<string> descInfo, InformationFile IR)
        {
            //(?:([0-9]+d[0-9]+)(?: \+ ([0-9]+d[0-9]+))*) (\w*)
            //[0-9]+d[0-9]+ (\w*)
            //(?:([0-9]+d[0-9]+)(?:(damage)\+ ([0-9]+d[0-9]+))*) (\w*) 
            //(?:([0-9]+d[0-9]+ \w* \+*)) //My current version of the regex
            //\dd\d (acid|bludgeoning|cold|fire|force|lightning|necrotic| piercing|poison|psychic|radiant|slashing|thunder|damage)( \+ \dd\d)? (acid|bludgeoning|cold|fire|force|lightning|necrotic|piercing|poison|psychic|radiant|slashing|thunder|damage)? xPath needs to be tested it worked on the tester
            Regex rgx = new Regex(@"(\d+d\d+ (acid|bludgeoning|cold|fire|force|lightning|necrotic|piercing|poison|psychic|radiant|slashing|thunder|damage)( damage)?( \+ | and \d+d\d+ \w+)?(\d+d\d+ \w+)?)");

            string o = "";

            foreach (var item in descInfo)
            {
                o = rgx.Match(item).Value;

                if (descInfo[descInfo.IndexOf(item)] == descInfo.Last() || string.IsNullOrEmpty(o))
                {
                    continue;
                }
                else
                {
                    IR.DamageEffect = o;
                }

            }

            if (string.IsNullOrEmpty(IR.DamageEffect))
            {
                IR.DamageEffect = "No Damage Found";
            }
            //string o = rgx.Match(descInfo[0]).Value;

            //foreach (var item in descInfo)
            //{
            //    foreach (Match match in rgx.Matches(item))
            //    {
            //        if (rgx.Matches(item).Count != 0)
            //        {
            //            IR.DamageEffect += $" + {match.Value}";
            //        }
            //        if (IR.DamageEffect.StartsWith(" "))
            //        {
            //            IR.DamageEffect = IR.DamageEffect.Remove(0, 3);
            //        }
            //    }
            //    if (IR.DamageEffect != null)
            //    {
            //        break;
            //    }
            //}

            return Task.CompletedTask;
        }
        private async static Task<Task> getAttackSave(InformationFile IR, List<string> desc)
        {
            string[] savingKeywords =
            {
                "Strength",
                "Dexterity",
                "Constitution",
                "Intelligence",
                "Wisdom",
                "Charisma",
            };

            //start of actual method//
            int savingThrow = 0;
            int index_of_keyword = -1;
            ;
            for (int i = 0; i < savingKeywords.Length; i++)
            {
                index_of_keyword = desc.FindIndex(x => x.Contains(savingKeywords[i]));
                if (index_of_keyword == -1)
                {
                    IR.AttackSave = "N/A";
                }
                else
                {
                    savingThrow = i;
                    break;
                }
            }



            //filter out the save//
            if (index_of_keyword != -1)
            {
                string save = desc[index_of_keyword].Substring(desc[index_of_keyword].IndexOf(savingKeywords[savingThrow]), $"{savingKeywords[savingThrow]} saving throw".Length);

                IR.AttackSave = $"{save.Substring(0, 3)} save";
            }

            return Task.CompletedTask;
        }
    }
}

