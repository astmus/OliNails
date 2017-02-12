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
using Java.Net;
using Java.Lang;

namespace OliNailsMobile
{
    interface IUploadAllData
    {
        string dataToUpload(string message);
    }
    interface IGotDataNowAct
    {
        void startAction(string message);
    }

    class UsbServer
    {
        private static UsbServer _instance = null;
        private ServerSocket _server;
        private volatile bool _isRunning = false;
        //private string _replyMessage = "Client#";
        private IUploadAllData _uploadAllData;
        private IGotDataNowAct _startSurvey;
        Thread _thread;
        volatile int _clientsCount;

        public static UsbServer Instance
        {
            get
            {
                return _instance ?? (_instance = new UsbServer());
            }
        }
        private UsbServer()
        {

        }

        public void Start(IGotDataNowAct receivedMessageHandler, IUploadAllData repliedMessageHandler)
        {
            _uploadAllData = repliedMessageHandler;
            _startSurvey = receivedMessageHandler;
            Start();
        }

        private void Start()
        {
            if (_isRunning) return;
            _thread = null;
            _isRunning = true;
            _thread = new Thread(new Runnable(() => {
                try
                {
                    _server = new ServerSocket(3128);
                    _server.ReuseAddress = true;
                    while (_isRunning)
                    {
                        Socket clientSocket = _server.Accept();
                        _clientsCount++;
                        new ClientConnectionHandler(clientSocket, _startSurvey, _uploadAllData).Start();
                    }
                    _isRunning = false;
                }
                catch 
                {
                	
                }
            }));
            _thread.Start();    
        }

        public void Stop()
        {
            _isRunning = false;
            _thread = null;
            if (_server != null)
            {
                try
                {
                    _server.Close();
                    _server = null;
                }
                catch
                {
                	
                }
            }
        }
    }
}