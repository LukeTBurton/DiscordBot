using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpBot.Modules.getFeats
{
    public class Information
    {
        public string Name { get; set; }
        public string Prerequisites { get; set; }
        public string Source { get; set; }
        public string Desc { get; set; }
        public string[] Traits { get; set; }
        //The Extras
        public string TblDescription { get; set; }
        public string[] TblHeaders { get; set; }
        public List<List<string>> TblRows { get; set; }
        public List<string> Extra { get; set; }
        public string table { get; set; }
    }
}
