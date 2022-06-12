using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using Android.Content;
using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text.Json;
using Xamarin.Essentials;


namespace IT123P_Machine_Problem
{
    [Activity(Label = "Receive A Message of Support")]
    public class RetrieveMessage : Activity
    {
        
        TextView txtMsg, txtID;
        ImageButton Receive, Report, LogOff;
        
        HttpWebResponse response;
        HttpWebRequest request;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Recieving);
            //Buttons
            Receive = FindViewById<ImageButton>(Resource.Id.recieve_BTN);
            Report = FindViewById<ImageButton>(Resource.Id.Report_Btn);
            LogOff = FindViewById<ImageButton>(Resource.Id.LogOut_BTN);
             
            txtMsg = FindViewById<TextView>(Resource.Id.message_Box);
            // hidden textview to store ID (for reporting)
            txtID = FindViewById<TextView>(Resource.Id.textView1);
            txtID.Visibility = Android.Views.ViewStates.Invisible;

            Receive.Click += getMessage;
            Report.Click += reportMessage;
            LogOff.Click += (o, i) =>
            {
                Intent goback = new Intent(this, typeof(MainActivity));
                StartActivity(goback);
            };
            //Auto fills out the entry on  load in
            buildMessage();
        }

        public void getMessage(object sender, EventArgs e)
        {
            buildMessage();
        }

        public void buildMessage()
        {
            // change ip to your pc's ipv4 address when testing on your end (type ipconfig in command prompt to find it)
            // also change directory to wherever you've stored the php files
            request = (HttpWebRequest)WebRequest.Create("http://192.168.0.17/SupportApp/REST/display_message.php");
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            var result = reader.ReadToEnd();
            //parse result to Json then get root element
            using JsonDocument doc = JsonDocument.Parse(result);
            JsonElement root = doc.RootElement;

            try // validation to check if messagetable has values
            {
                var data = root[0];
                string message = data.GetProperty("message").ToString();
                string id = data.GetProperty("messageID").ToString();

                // set values from php
                txtMsg.Text = message;
                txtID.Text = id;
                Toast.MakeText(this, "Someone wants to tell you this...", ToastLength.Long).Show();
            }
            catch (IndexOutOfRangeException) // error to catch if messagetable is empty
            {
                Toast.MakeText(this, "There are no messages available at the moment...", ToastLength.Long).Show();
                return;
            }
        }

        public void reportMessage(object sender, EventArgs e)
        {
            string user = Intent.GetStringExtra("Name");
            string reportedID = txtID.Text;
            string reporter = user;
            string webRequest = "http://192.168.0.17/SupportApp/REST/report_message.php?messageID=" + reportedID + "&username=" + reporter;
            request = (HttpWebRequest)WebRequest.Create(webRequest);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string res = reader.ReadToEnd();
            Toast.MakeText(this, res, ToastLength.Long).Show();
        }

        
    }
}