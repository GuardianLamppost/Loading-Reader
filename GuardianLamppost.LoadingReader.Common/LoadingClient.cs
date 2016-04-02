using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GuardianLamppost.LoadingReader.Common.Entitites;
using GuardianLamppost.LoadingReader.Common.Readers;
using System.IO;
using System.Net.Http;
using HtmlAgilityPack;

namespace GuardianLamppost.LoadingReader.Common {
    public class LoadingClient {
        public ForumReader ForumReader { get; set; }
        private CookieContainer Session { get; set; }
        public bool IsCategorisedView { get; set; }
        public LoadingClient() {
            Session = new CookieContainer();
            var initialRequest = WebRequest.CreateHttp("http://loading.se/forum.php");
            initialRequest.CookieContainer = Session;
            var initialRequestTask = initialRequest.GetResponseAsync();
            if (!initialRequestTask.Wait(10000)) {
                throw new Exception("Kunde inte kontakta loading.se");
            }
            initialRequestTask.Result.Dispose();
            ForumReader = new ForumReader(initialRequest.CookieContainer, this);
        }

        public async Task<bool> Login(string username, string password) {
            var loginRequest = WebRequest.CreateHttp("http://loading.se/forum.php");
            loginRequest.Method = "POST";
            loginRequest.CookieContainer = Session;
            var loginString = string.Format("login_nick={0}&login_pwd={1}&login_auto=1", WebUtility.UrlEncode(username), WebUtility.UrlEncode(password));
            var loginBytes = Encoding.GetEncoding("windows-1252").GetBytes(loginString);
            loginRequest.ContentType = "application/x-www-form-urlencoded";
            using (var stream = await loginRequest.GetRequestStreamAsync()) {
                stream.Write(loginBytes, 0, loginBytes.Count());
            }
            var response = await loginRequest.GetResponseAsync();
            response.Dispose();
            return Session.GetCookies(new Uri("http://loading.se"))["auto_nick"] != null;
        }
    }
}
