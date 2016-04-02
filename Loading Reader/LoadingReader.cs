using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GuardianLamppost.LoadingReader.Common;

namespace GuardianLamppost.LoadingReader {
    public class LoadingReader {
        private static LoadingClient loadingClient;
        public static LoadingClient LoadingClient {
            get {
                if (loadingClient == null) {
                    loadingClient = new LoadingClient();
                }
                return loadingClient;
            }
        }

    }
}