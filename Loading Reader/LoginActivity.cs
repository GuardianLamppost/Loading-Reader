using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using GuardianLamppost.LoadingReader.Common;
using System.ComponentModel;
using System.Threading.Tasks;


namespace GuardianLamppost.LoadingReader {
    [Activity(Label = "LoadingReader", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        private ProgressDialog ProgressDialog { get; set; }
        private LoadingClient Client { get; set; }
        private EditText UsernameField { get; set; }
        private EditText PasswordField { get; set; }

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

            ProgressDialog = new ProgressDialog(this);
            Client = new LoadingClient();
            UsernameField = FindViewById<EditText>(Resource.Id.usernameField);
            PasswordField = FindViewById<EditText>(Resource.Id.passwordField);
            FindViewById<Button>(Resource.Id.loginButton).Click += LoginButton_Click;
        }

        private void LoginButton_Click(object sender, EventArgs e) {
            StartActivity(new Intent(this, typeof(ThreadListActivity)));
            /*ProgressDialog.SetTitle("Loggar in!");
            ProgressDialog.Show();
            var context = this;
            var loginTask = Task.Run(() => Client.Login(UsernameField.Text, PasswordField.Text)).ContinueWith((task) => {
                RunOnUiThread(() => {
                    ProgressDialog.Hide();
                    if (task.Result) {
                        RunOnUiThread(() => Toast.MakeText(context, "Det gick bra :)", ToastLength.Long).Show());
                    } else {
                        RunOnUiThread(() => Toast.MakeText(context, "Kunde inte logga in :(", ToastLength.Long).Show());
                    }
                });
            });*/


        }
    }
}

