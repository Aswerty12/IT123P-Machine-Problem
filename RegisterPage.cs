using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT123P_Machine_Problem
{
    [Activity(Label = "Register")]
    public class RegisterPage : Activity
    {
        EditText username, password;
        ImageButton go;
        
        HttpWebResponse response;
        HttpWebRequest request;
        string res = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //set layout
            SetContentView(Resource.Layout.Register);
            // Create your application here
            username = FindViewById<EditText>(Resource.Id.Username_Line);
            password = FindViewById<EditText>(Resource.Id.Password_Line);
            go = FindViewById<ImageButton>(Resource.Id.Main_Btn);

            

           go.Click += newAccount;
            
        }

        public void newAccount(object sender, EventArgs e)
        {
            string newpword = password.Text;
            string newuser = username.Text;
            //hashes new password
            Hashing h = new Hashing();
            string hashedpass = h.HashString(newpword);
            string toSend = "http://192.168.0.17/SupportApp/REST/register_account.php?username=" + newuser + "&password=" + hashedpass;

            var checkif = checkif_blank();
            if (checkif)
            {
                request = (HttpWebRequest)WebRequest.Create(toSend);
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();
                Toast.MakeText(this, res, ToastLength.Long).Show();
                //Clear once the user submitted a message
                //Clear();
                //Alt Logic Login with confirmation that user is now registerd
                Intent i = new Intent(this, typeof(LandingPage));
                i.PutExtra("Name", newuser);
                StartActivity(i);
            }
            else
            {
                Toast.MakeText(this, "Please fill all fields", ToastLength.Long).Show();
            }
        }

        public bool checkif_blank()
        {
            if (username.Text == "" ||
                password.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Clear()
        {
            username.Text = "";
            password.Text = "";
        }
        //Deprecated Feature
        public void GoBack(object sender, EventArgs e)
        {
            Finish();
        }
    }
   
}