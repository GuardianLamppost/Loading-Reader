using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GuardianLamppost.LoadingReader.Common.Entitites;
using GuardianLamppost.LoadingReader.Common.Readers;
using System.IO;

namespace GuardianLamppost.LoadingReader.Common {
    public class LoadingClient {
        private ForumReader ForumReader { get; set; }
        private CookieContainer Session { get; set; }
        public LoadingClient() {
            Session = new CookieContainer();
            var initialRequest = WebRequest.CreateHttp("http://loading.se/forum.php");
            initialRequest.CookieContainer = Session;
            var initialRequestTask = initialRequest.GetResponseAsync();
            if (!initialRequestTask.Wait(10000)) {
                throw new Exception("Kunde inte kontakta loading.se");
            }
            var result = initialRequestTask.Result;

            ForumReader = new ForumReader(initialRequest.CookieContainer);
        }

        public async Task<bool> Login(string username, string password) {
            try {
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
                return Session.GetCookies(new Uri("http://loading.se"))["auto_nick"] != null;
            } catch (Exception e) {
                return false;
            }
        }

        public IEnumerable<ThreadListThread> GetThreads() {
            var threads = ForumReader.GetThreads();
            return threads;
        }
    }
}
