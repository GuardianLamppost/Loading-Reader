using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardianLamppost.LoadingReader.Common.Entitites {
    public class CategoryListForum : CategoryListItem {
        public Image StatusIcon { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public string LastThreadTitle { get; set; }
        public string LastChangeAt { get; set; }
        public string LastChangeBy { get; set; }
        public bool LastChangeUserOnline { get; set; }

        public CategoryListForum(HtmlNode forumNode, CachedImageFetcher cachedImageFetcher) {
            var statusImageNode = forumNode.FirstChild.Descendants().Single(x => x.Name == "img");
            StatusIcon = cachedImageFetcher.FetchImage(statusImageNode.Attributes["src"].Value);
            var hrefNode = forumNode.Descendants().Single(x => x.Name == "a" && x.Attributes["class"]?.Value == "forum_cat_forum");
            Title = hrefNode.InnerText;
            Id = hrefNode.Attributes["href"].Value.Replace("loading.se/forum.php?forum_id=", string.Empty);
            var lastPostNode = hrefNode.ParentNode.Descendants().SingleOrDefault(x => x.Name == "a" && x.Attributes["class"]?.Value == "forum_info2");
            if (lastPostNode != null) {
                LastThreadTitle = lastPostNode.InnerText;
                LastChangeAt = forumNode.LastChild.Descendants().FirstOrDefault(x => x.Attributes["class"]?.Value == "forum_info2")?.InnerText.Replace("Ändrad: ", string.Empty);
            }
            var lastChangeByNode = forumNode.LastChild.Descendants().FirstOrDefault(x => x.Attributes["class"] != null && x.Attributes["class"].Value.StartsWith("forum_user"));
            if (lastChangeByNode != null) {
                LastChangeBy = lastChangeByNode.InnerText;
                LastChangeUserOnline = lastChangeByNode.Attributes["class"].Value == "forum_user_on";
            }
        }
    }
}
