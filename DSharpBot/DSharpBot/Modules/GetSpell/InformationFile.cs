using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpBot.Modules.GetSpell
{
    public class InformationFile
    {
        public string Level { get; set; }
        public string Name { get; set; }
        public string CastingTime { get; set; }
        public string RangeArea { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string School { get; set; }
        public string AttackSave { get; set; }
        public string DamageEffect { get; set; }
        public string[] Description { get; set; }
        public string Classes { get; set; }
        public int descIndex { get; set; }
        public string tableContents { get; set; }
    }
}
