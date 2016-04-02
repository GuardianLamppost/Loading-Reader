using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GuardianLamppost.LoadingReader.Common {
    public class CachedImageFetcher {
        private Dictionary<string, Image> FetchedImages { get; set; }
        public CachedImageFetcher() {
            FetchedImages = new Dictionary<string, Image>();
        }

        public Image FetchImage(string relativePath) {
            if (!FetchedImages.ContainsKey(relativePath)) {
                var request = WebRequest.CreateHttp(new Uri(new Uri("http://loading.se"), relativePath));
                var response = request.GetResponseAsync();
                if (response.Wait(10000)) {
                    using (var byteData = new MemoryStream()) {
                        using (var stream = response.Result.GetResponseStream()) {
                            stream.CopyTo(byteData);
                        }
                        response.Result.Dispose();
                        FetchedImages.Add(relativePath, new Image { RelativePath = relativePath, ImageData = byteData.ToArray() });
                    }
                } else {
                    return null;
                }
            }
            return FetchedImages[relativePath];
        }
    }
}
