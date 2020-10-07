using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSharpBot.Modules
{
    public class getDoc
    {
        public async static Task<HtmlDocument> getHtmlDoc(string URI)
        {
            HtmlDocument doc = new HtmlDocument();

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent: Other");
                string Html = client.DownloadString(URI);
                doc.LoadHtml(Html);
            }

            return doc;
        }
    }
}
