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
using Java.Lang;
using Java.Net;
using Java.IO;
using Android.Telephony;

namespace OliNailsMobile
{

    class ClientConnectionHandler : Thread
    {
        private Socket _socket;
        private IUploadAllData _uploadAllData;
        private IGotDataNowAct _startSurvey;

        public ClientConnectionHandler(Socket socket, IGotDataNowAct receivedMessageHandler, IUploadAllData repliedMessageHandler)
        {
            _socket = socket;
            _uploadAllData = repliedMessageHandler;
            _startSurvey = receivedMessageHandler;
        }

        public override void Run()
        {
            base.Run();
            try
            {
                BufferedReader inReader = new BufferedReader(new InputStreamReader(_socket.InputStream));
                PrintStream outWriter = new PrintStream(_socket.OutputStream, true);

                string data = inReader.ReadLine();
                string [] requestLine = data.Split(';');
                switch (requestLine[0])
                {                    
                    case "send_sms":
                        try
                        {
                            SmsManager.Default.SendTextMessage(requestLine[1], null, requestLine[2],null, null);
                            _startSurvey.startAction(string.Format("sent '{0}' to {1} was ok", requestLine[2], requestLine[1]));
                            outWriter.Println("sent to "+ requestLine[1] + " ok");
                        }
                        catch (System.Exception ex)
                        {
                            _startSurvey.startAction("sent to " + requestLine[1]+" throw exteption with message = " + ex.Message);
                            outWriter.Println("sent to " + requestLine[1] + " failed");
                        }                        
                        
                        break;
                }

                _socket.Close();
            }
            catch
            {

            }
        }
    }
}