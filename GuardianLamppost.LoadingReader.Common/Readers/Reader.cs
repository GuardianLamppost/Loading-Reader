using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GuardianLamppost.LoadingReader.Common.Readers {
    public abstract class Reader {
        protected bool IsLoggedIn { get; set; }
        protected CookieContainer Session { get; set; }

        public Reader(CookieContainer session) {
            Session = session;
        }
    }
}
