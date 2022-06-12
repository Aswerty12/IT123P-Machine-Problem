using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT123P_Machine_Problem
{
    [Activity(Label = "LandingPage")]
    public class LandingPage : Activity
    {
        string login_name;
        TextView txt1;
        ImageButton send, receive, back;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Landing_Page);
            //variable is just here because sending variables across events is a PITA
            login_name = Intent.GetStringExtra("Name");
            txt1 = FindViewById<TextView>(Resource.Id.textView1);
            txt1.Text = login_name;

            //ImageButtons
            send = FindViewById<ImageButton>(Resource.Id.Send_BTN);
            receive = FindViewById<ImageButton>(Resource.Id.Recieve_BTN);
            back = FindViewById<ImageButton>(Resource.Id.LogOut_BTN);
            //button logic
            

            send.Click += (o, i) =>
            {
                Intent sending = new Intent(this, typeof(SendingPage));
                sending.PutExtra("Name", login_name);
                StartActivity(sending);
            };
            receive.Click += (o, i) =>
            {
                Intent receiving = new Intent(this,typeof(RetrieveMessage));
                receiving.PutExtra("Name", login_name);
                StartActivity(receiving);
            };
            back.Click += (o, i) =>
            {
                Intent goback = new Intent(this, typeof(MainActivity));
                StartActivity(goback);
            };
        }
    }
}