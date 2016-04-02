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
    [Activity(Label = "LoadingReader", MainLauncher = true, Icon = "@drawable/icon", Exported = true)]
    public class LoginActivity : Activity {
        private ProgressDialog ProgressDialog { get; set; }
        private EditText UsernameField { get; set; }
        private EditText PasswordField { get; set; }

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

            ProgressDialog = new ProgressDialog(this);

            UsernameField = FindViewById<EditText>(Resource.Id.usernameField);
            PasswordField = FindViewById<EditText>(Resource.Id.passwordField);
            FindViewById<Button>(Resource.Id.loginButton).Click += LoginButton_Click;
        }

        private void LoginButton_Click(object sender, EventArgs e) {

            ProgressDialog.SetTitle("Loggar in!");
            ProgressDialog.Show();
            var context = this;

            Task.Run(() => LoadingReader.LoadingClient.Login(UsernameField.Text, PasswordField.Text)).ContinueWith((task) => {
                RunOnUiThread(() => {
                    ProgressDialog.Hide();
                    if (task.Result) {
                        var mainActivityIntent = new Intent(this, typeof(MainActivity));
                        StartActivity(mainActivityIntent);
                    } else {
                        RunOnUiThread(() => Toast.MakeText(context, "Kunde inte logga in :(", ToastLength.Long).Show());
                    }
                });
            });



        }
    }
}

