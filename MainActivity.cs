using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Net;
using System.IO;
using Android.Content;

namespace IT123P_Machine_Problem
{
    [Activity(Label = "Machine Problem", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    { 
        EditText edit1, edit2;
        ImageButton Lin;
        TextView register;
        HttpWebResponse response;
        HttpWebRequest request;
        string res = "", uname = "", pword = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            edit1 = FindViewById<EditText>(Resource.Id.Username_Line);
            edit2 = FindViewById<EditText>(Resource.Id.Password_Line);
            register = FindViewById<TextView>(Resource.Id.Register_Clck);
            Lin = FindViewById<ImageButton>(Resource.Id.Main_Btn);

            Lin.Click += Login;
            register.Click += Registration;

        }

        public void Login(object sender, EventArgs e)
        {
            pword = edit2.Text;
            //Uses the hashing class to get a hashed password
            Hashing h = new Hashing();
            pword = h.HashString(pword);
            uname = edit1.Text;
            //Change to your ip adress and ports.
            //Again I stress that this needs the correct ports and IP
            request = (HttpWebRequest)WebRequest.Create("http://192.168.0.17/SupportApp/REST/user_login.php?uname=" + uname + " &password=" + pword);
            request.Proxy = null;
            request.Timeout = 2000;
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            Toast.MakeText(this, res, ToastLength.Long).Show();

            if (res.Contains("OK!"))
            {
                Intent i = new Intent(this, typeof(LandingPage));
                i.PutExtra("Name", uname); 
                StartActivity(i);
                
            }
        }

        public void Registration(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(RegisterPage));
            StartActivity(i);
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}