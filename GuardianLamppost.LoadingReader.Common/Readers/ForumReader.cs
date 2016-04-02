using GuardianLamppost.LoadingReader.Common.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Diagnostics;

namespace GuardianLamppost.LoadingReader.Common.Readers {
    public class ForumReader : Reader {
        string BaseUrl = "http://loading.se/forum.php";

        public ForumReader(CookieContainer session) :
            base(session) {
        }

        public async Task<List<ThreadListThread>> GetThreads() {
            var request = WebRequest.CreateHttp("http://loading.se/forum.php");
            request.CookieContainer = Session;
            Debug.WriteLine("Starting request!");
            var response = await request.GetResponseAsync();

            Debug.WriteLine("Ending request!");
            var htmlDocument = new HtmlDocument();
            using (var stream = response.GetResponseStream()) {
                htmlDocument.Load(stream, Encoding.GetEncoding("windows-1252"));
            }
            response.Dispose();
            var nodes = htmlDocument.DocumentNode.Descendants().Where(node => node.Name == "a" && node.Attributes["class"]?.Value == "forum_forum_thread");
            var cachedImageFetcher = new CachedImageFetcher();
            return nodes.Select(x => new ThreadListThread(x, cachedImageFetcher)).ToList();
        }
    }
}
