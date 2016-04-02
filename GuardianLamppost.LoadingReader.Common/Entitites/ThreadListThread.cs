using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace GuardianLamppost.LoadingReader.Common.Entitites {

    public class ThreadListThread {
        public Image StatusIcon { get; set; }
        public string Subject { get; set; }
        public string CreatedBy { get; set; }
        public bool IsCreatedByUserOnline { get; set; }
        public string CreatedAt { get; set; }
        public string LastPostBy { get; set; }
        public bool IsLastPostUserOnline { get; set; }
        public string LastPostAt { get; set; }
        public string ThreadId { get; set; }
        public string JumpToLink { get; set; }

        public ThreadListThread(HtmlNode threadNode, CachedImageFetcher cachedImageFetcher) {
            try {
                var statusIconPath = threadNode.ParentNode.ParentNode.ChildNodes.Single(x => x.Attributes["class"]?.Value == "forum_forum_tbr_td thread_status").FirstChild.Attributes["src"]?.Value;
                StatusIcon = cachedImageFetcher.FetchImage(statusIconPath);
                Subject = threadNode.InnerText;

                var infoNode = threadNode.ParentNode.LastChild;
                var createdByNode = infoNode.FirstChild.Descendants().SingleOrDefault(x => x.Attributes["class"] != null && x.Attributes["class"].Value.StartsWith("forum_user"));
                CreatedAt = infoNode.FirstChild.FirstChild.InnerText.Replace(" av ", string.Empty);
                CreatedBy = createdByNode != null ? createdByNode.InnerText : "Okänd";
                IsCreatedByUserOnline = createdByNode != null ? createdByNode.Attributes["class"].Value == "forum_user_on" : false;

                var threadModifiedNode = infoNode.ParentNode.ParentNode.ChildNodes.SingleOrDefault(x => x.Attributes["class"].Value == "forum_forum_tbr_td thread_modified");
                if (threadModifiedNode.HasChildNodes) {
                    var lastPostUserNode = threadModifiedNode.ChildNodes.SingleOrDefault(x => x.Name == "a");
                    LastPostAt = threadModifiedNode.FirstChild.InnerText.Replace("Ändrad: ", string.Empty);
                    LastPostBy = lastPostUserNode != null ? lastPostUserNode.InnerText : "Okänd";
                    IsLastPostUserOnline = lastPostUserNode != null ? lastPostUserNode.Attributes["class"].Value == "forum_user_on" : false;
                } else {
                    LastPostAt = string.Empty;
                    LastPostBy = "Okänd";
                }

                ThreadId = threadNode.Attributes["href"].Value.Replace("forum.php?thread_id=", string.Empty);

                var lastReadPageNode = threadNode.ParentNode.Descendants().SingleOrDefault(x => x.Name == "img" && x.Attributes["src"].Value == "/gfx/forum_jump_p.gif");
                if (lastReadPageNode != null && lastReadPageNode.ParentNode.Attributes["src"] != null) {
                    var jumpToLink = lastReadPageNode.ParentNode.Attributes["src"].Value;
                    JumpToLink = jumpToLink.Substring(jumpToLink.LastIndexOf('&') + 1).Replace("jump=", string.Empty);
                }
            } catch (Exception e) {
                e.ToString();
            }
        }

    }
}
