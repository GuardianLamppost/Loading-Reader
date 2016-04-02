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
        public LoadingClient Client { get; set; }
        public ForumReader(CookieContainer session, LoadingClient client) :
            base(session) {
            Client = client;
        }

        private async Task<HtmlDocument> ReadForumPage() {
            try {
                var request = WebRequest.CreateHttp("http://loading.se/forum.php");
                request.CookieContainer = Session;
                var response = await request.GetResponseAsync();
                var htmlDocument = new HtmlDocument();
                using (var stream = response.GetResponseStream()) {
                    htmlDocument.Load(stream, Encoding.GetEncoding("windows-1252"));
                }
                response.Dispose();
                return htmlDocument;
            } catch (Exception e) {
                e.ToString();
                throw e;
            }
        }

        public async Task<List<ThreadListThread>> GetThreads() {
            var htmlDocument = await ReadForumPage();
            if (IsCategorisedView(htmlDocument)) {
                //Forum view is categorised, return null!
                Client.IsCategorisedView = true;
                return null;
            }

            var nodes = htmlDocument.DocumentNode.Descendants().Where(node => node.Name == "a" && node.Attributes["class"]?.Value == "forum_forum_thread");
            var cachedImageFetcher = new CachedImageFetcher();
            return nodes.Select(x => new ThreadListThread(x, cachedImageFetcher)).ToList();
        }

        public async Task<List<CategoryListItem>> GetCategories() {
            var htmlDocument = await ReadForumPage();
            if (!IsCategorisedView(htmlDocument)) {
                //Forum view is categorised, return null!
                Client.IsCategorisedView = false;
                return null;
            }
            var nodes = htmlDocument.DocumentNode.Descendants().Single(x => x.Attributes["class"]?.Value == "forum_list").Descendants().Where(node => node.Name == "td" && node.Attributes["class"]?.Value == "forum_cat_sep");


            return nodes.SelectMany(x => CategoryListCategory.GetCategoryList(x)).ToList();
        }

        private bool IsCategorisedView(HtmlDocument htmlDocument) {
            return htmlDocument.DocumentNode.Descendants().Any(x => x.Name == "table" && x.Attributes["class"]?.Value == "forum_list");
        }


    }
}
