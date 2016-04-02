using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardianLamppost.LoadingReader.Common.Entitites {
    public class CategoryListCategory : CategoryListItem {
        public string Title { get; set; }

        public static List<CategoryListItem> GetCategoryList(HtmlNode categoryNode) {
            List<HtmlNode> forumNodes = new List<HtmlNode>();
            var currentNode = categoryNode.ParentNode.NextSibling;
            while (currentNode != null && currentNode.Attributes["class"] != null) {
                forumNodes.Add(currentNode);
                currentNode = currentNode.NextSibling;
            }

            var title = String.Join(" -> ", categoryNode.ChildNodes.Where(x => x.Attributes["class"]?.Value == "forum_cat_desc").Select(x => x.InnerText));
            var cachedImageFetcher = new CachedImageFetcher();
            var forums = forumNodes.Select(x => new CategoryListForum(x, cachedImageFetcher));
            return new CategoryListItem[] { new CategoryListCategory { Title = title } }.Concat(forums.Cast<CategoryListItem>()).ToList();
        }

    }
}
