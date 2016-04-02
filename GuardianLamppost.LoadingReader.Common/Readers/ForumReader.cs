using GuardianLamppost.LoadingReader.Common.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace GuardianLamppost.LoadingReader.Common.Readers {
    public class ForumReader : Reader {
        string BaseUrl = "http://loading.se/forum.php";

        public ForumReader(CookieContainer session) :
            base(session) {
        }

        public IEnumerable<ThreadListThread> GetThreads() {
            var request = WebRequest.CreateHttp(BaseUrl);
            request.CookieContainer = Session;
            var task = request.GetResponseAsync();
            if (!task.Wait(10000)) {
                throw new Exception("Kunde inte kontakta loading.se!");
            }
            var cookies = request.CookieContainer.GetCookies(new Uri(BaseUrl));
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(task.Result.GetResponseStream(), Encoding.GetEncoding("windows-1252"));
            var nodes = htmlDocument.DocumentNode.Descendants().Where(node => node.Name == "a" && node.Attributes["class"]?.Value == "forum_forum_thread");
            var entities = nodes.Select(x => new ThreadListThread(x)).ToList();
            return entities;
        }
    }
}
