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
    [Activity(Label = "SendingPage")]
    public class SendingPage : Activity
    {
        EditText editMessage;
        TextView User;
        ImageButton sendIt, goBack;
        
        HttpWebResponse response;
        HttpWebRequest request;
        String message = "", username = "", res = "", login_name = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //set layout
            SetContentView(Resource.Layout.sending);
            //get name of who login through Intent
            login_name = Intent.GetStringExtra("Name");
            //instantiate widgets
            User= FindViewById<TextView>(Resource.Id.textView2);
            editMessage = FindViewById<EditText>(Resource.Id.message_Box);
            sendIt = FindViewById<ImageButton>(Resource.Id.Send_BTN);
            goBack = FindViewById<ImageButton>(Resource.Id.LogOut_BTN);        
            User.Text = login_name;

            goBack.Click += (o, i) =>
            {
                Intent goback = new Intent(this, typeof(MainActivity));
                StartActivity(goback);
            };//Logs out of course

            sendIt.Click += this.AddMessage;
        }
        

        public void AddMessage(object sender, EventArgs e)
        {
            message = editMessage.Text;
            username = User.Text;
            string toSend = "http://192.168.0.17/SupportApp/REST/add_message.php?message=" + message + "&username=" + username;

            var checkif = checkif_blank();
            if (checkif)
            {
                request = (HttpWebRequest)WebRequest.Create(toSend);
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();
                Toast.MakeText(this, res, ToastLength.Long).Show();
                //Clear once the user submitted a message
                Clear();
            }
            else
            {
                Toast.MakeText(this,"Please fill the message",ToastLength.Long).Show();
            }
        }
        
        public bool checkif_blank()
        {
            if(editMessage.Text == "" ||
                User.Text == "")
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
            editMessage.Text = "";
        }


     

       
    }
}