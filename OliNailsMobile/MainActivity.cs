using Android.App;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using Android.Text.Method;
using Java.Lang;
using System;
using Android.Net.Wifi;
using Android.Content;
using Android.Text.Format;

namespace OliNailsMobile
{
	[Activity(Label = "OliNailsMobile", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IUploadAllData, IGotDataNowAct
    {
        Handler _handler;
        TextView _logView;
        TextView _ipText;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            _handler = new Handler();

            _logView = FindViewById<TextView>(Resource.Id.logView);
            _logView.MovementMethod = new ScrollingMovementMethod();
            _ipText = FindViewById<TextView>(Resource.Id.ipText);
            WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Context.WifiService);
            int ip = wifiManager.ConnectionInfo.IpAddress;
            _ipText.Text = Formatter.FormatIpAddress(ip)+":3128";
        }

        private void logToView(string text)
        {
            _handler.Post(new Runnable(() =>
            {
                _logView.Append("\n" + text);
            }));
        }

        protected override void OnResume()
        {
            base.OnResume();
            UsbServer.Instance.Start(this, this);
            logToView("\n server started");
        }

        protected override void OnPause()
        {
            base.OnPause();
            UsbServer.Instance.Stop();
            logToView("\n server stopped");
        }

        public string dataToUpload(string message)
        {
            logToView("Uploading Data to PC ....");
            if (Looper.MyLooper() == Looper.MainLooper)
            {
                string ml = "main looper";
            }
            else
            {
                try
                {
                    Thread.Sleep(1000);
                }
                catch (InterruptedException e)
                {                    
                }
            }
            logToView("Done Uploading...");
            return "Send From Main Activity ... ";
        }

        public void startAction(string message)
        {            
            logToView("result = "+message);
        }        
    }
}

