using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpBot.Modules.getFeats
{
    class embedBuilder
    {
        internal static DiscordEmbedBuilder startBuilder(Information info, int amount)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            //Name
            builder.AddField("Name", info.Name);

            //Prerequisites
            builder.AddField("Prerequisites", info.Prerequisites);

            //Desc
            builder.AddField("Description", info.Desc);

            //Traits
            for (int i = 0; i < info.Traits.Length; i++)
            {
                builder.AddField($"Trait {i + 1}:", info.Traits[i]);
            }

            //Optional
            if (info.TblHeaders != null)
            {
                //TblDesc
                builder.AddField("Table Description", info.TblDescription);

                //Table
                builder.AddField("Table", info.table);

                //Extra
                foreach (var item in info.Extra)
                {
                    builder.AddField("Extra", item);
                }
            }

            //Source
            builder.AddField("Source", info.Source);

            return builder;
        }
    }
}
