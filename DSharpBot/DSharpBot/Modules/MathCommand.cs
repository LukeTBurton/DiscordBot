using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace DSharpBot.Modules
{
    public class MathCommand
    {
        // this command will use our custom type, for which have 
        // registered a converter during initialization
        [Command("math"), Description("Does basic math. cause im dumb")]
        public async Task Math(CommandContext ctx, [RemainingText][Description("input numbers")] string input)
        {
            Regex rgx = new Regex(@"(\(?(?<number>\d{0,9999}[\^\.]?\d{0,9999})(?<operator1>[\+\*\-\/\%]?)(?<number2>\d{0,9999}[\^\.]?\d{0,9999})(?<operator2>[\*\+\-\/\%]?)(\d{0,9999}[\^\.]?\d{0,9999})\)?)[\+\-\/\*\%]?");

            object Res = Result(rgx.Matches(input));

            var emoji = DiscordEmoji.FromName(ctx.Client, ":thinking:");
            await ctx.RespondAsync($"{emoji} The result is {Res.ToString()}");
        }

        private object Result(MatchCollection matchCollections)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (Match item in matchCollections)
            {
                if (item.Value.Contains("^"))
                {
                    sBuilder.Append(ToThePower(item));
                }
                else
                {
                    sBuilder.Append(item);
                }
            }

            string expression = sBuilder.ToString();

            expression = Regex.Replace(
                expression,
                @"\d+(\.\d+)?",
                m => {
                    var x = m.ToString();
                    return x.Contains(".") ? x : string.Format("{0}.0", x);
                });

            var loDataTable = new DataTable();
            var result = loDataTable.Compute(expression, string.Empty);

            return result;
        }

        private static string ToThePower(Match item)
        {
            string returnVal = item.Value;
            double value1 = 0.0;
            double value2 = 0.0;

            string repla1 = item.Groups["number"].Value;

            if (repla1.Contains("^"))
            {
                string[] Split = repla1.Split('^');
                value1 = System.Math.Pow(Convert.ToDouble(Split[0]), Convert.ToDouble(Split[1]));
                returnVal = returnVal.Replace(Convert.ToString(repla1), Convert.ToString(value1));
                //item.Value.Replace($"{repla1}", value1);
            }
            string repla2 = item.Groups["number2"].Value;

            if (repla2.Contains("^"))
            {
                string[] Split = repla2.Split('^');
                value2 = System.Math.Pow(Convert.ToDouble(Split[0]), Convert.ToDouble(Split[1]));
                returnVal = returnVal.Replace(Convert.ToString(repla2), Convert.ToString(value2));
            }

            return returnVal;
        }
    }
}
