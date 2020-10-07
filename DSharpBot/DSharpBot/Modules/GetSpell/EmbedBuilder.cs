using System;
using System.Collections.Generic;
using System.Text;

using DSharpPlus.CommandsNext;
using DSharpPlus;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using DSharpBot.Modules.GetSpell;

namespace DSharpBot.Modules
{
    public class EmbedClass
    {
        public static DiscordEmbedBuilder startBuilder(InformationFile IR, HtmlDocument doc)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            for (int i = 0; i < 11 + IR.Description.Length - 1; i++)
            {
                if (IR.tableContents == null && i == 9)
                    i++;
                if (i >= 11)                
                    IR.descIndex++;
                
                builder.AddField(fieldBuilderName(i, IR, doc), fieldBuilderNameValue(i, IR, doc));
            }

            return builder;
        }
        private static string fieldBuilderName(int i, InformationFile IR, HtmlDocument doc = null)
        {
            if (i == 0)//Name
            {
                return "Name";
            }
            else if (i == 1)//Level
            {
                return "Level";
            }
            else if (i == 2)//Casting Time
            {
                return "Casting Time";
            }
            else if (i == 3)//Range/Area
            {
                return "Range/Area";
            }
            else if (i == 4)//Components
            {
                return "Components";
            }
            else if (i == 5)//Duration
            {
                return "Duration";
            }
            else if (i == 6)//School
            {
                return "School";
            }
            else if (i == 7)//Attack/Save
            {
                return "Attack/Save";
            }
            else if (i == 8)//Damage/Effect
            {
                return "Damage/Effect";
            }
            else if (i == 9)
            {
                if (IR.tableContents != null)
                {
                    return "Table";
                }

            }
            else if (i >= 10)//Description
            {
                return "Description";
            }

            return null;
        }



        private static string fieldBuilderNameValue(int i, InformationFile IR, HtmlDocument doc = null)
        {
            if (i == 0)//Name
            {
                return IR.Name;
            }
            else if (i == 1)//Level
            {
                return IR.Level;
            }
            else if (i == 2)//Casting Time
            {
                return IR.CastingTime;
            }
            else if (i == 3)//Range/Area
            {
                return IR.RangeArea;
            }
            else if (i == 4)//Components
            {
                return IR.Components;
            }
            else if (i == 5)//Duration
            {
                return IR.Duration;
            }
            else if (i == 6)//School
            {
                return IR.School;
            }
            else if (i == 7)//Attack/Save
            {
                return IR.AttackSave;
            }
            else if (i == 8)//Damage/Effect
            {
                if (IR.DamageEffect != null && IR.DamageEffect != "" && !string.IsNullOrWhiteSpace(IR.DamageEffect))
                {
                    return IR.DamageEffect;
                }
                else
                {
                    return "N/A";
                }

            }
            else if (i == 9)
            {
                if (IR.tableContents != null)
                {
                    return IR.tableContents;
                }

            }
            else if (i >= 10)//Description
            {
                if (IR.descIndex < IR.Description.Length)
                {
                    if ((IR.Description[IR.descIndex] != null))
                    {
                        try
                        {
                            if (IR.descIndex == 0)
                            {
                                return IR.Description[IR.descIndex];
                            }
                            else
                            {
                                return IR.Description[IR.descIndex];
                            }
                        }
                        catch
                        {

                        }
                        
                    }
                }
                // , RunMode = RunMode.Async
            }

            return null;
        }
    }
}
