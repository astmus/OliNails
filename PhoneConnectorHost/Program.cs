using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace PhoneConnectorHost
{
    class Program
    {
        static void Main(string[] args)
        {   
            if (args.Length == 0)
            {
                Console.WriteLine("Empty parameters for phone and sms text");
                Console.ReadKey();
                return;
            }
            
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.FileName = @"c:\Program Files (x86)\Android\android-sdk\platform-tools\send.bat";
            proc.StartInfo.Arguments = String.Format("{0} \"{1}\"","+380939372858","Вы записались на 13:00 19.02.2017");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;            
            proc.Start();
            proc.WaitForExit();
            string res = "";
            while (res.IndexOf("completed") == -1)
                res = proc.StandardOutput.ReadLine();
            //proc.StandardInput.WriteLine(String.Format("am start -a android.intent.action.SENDTO -d sms:{0} --es sms_body \"{1}\" --ez exit_on_sent true",args[0], "olinails.com Вы успешно записались на "));
            //string s = String.Format("am start -a android.intent.action.SENDTO -d sms:{0} --es sms_body \"{1}\" --ez exit_on_sent true", args[0], "olinails.com ");
            //proc.StandardInput.WriteLine(s);
            //proc.StandardInput.Flush();
            //Thread.Sleep(1000);
            //proc.StandardInput.WriteLine(String.Format("input text \"{0}.%sJdem%Vas.\"",args[1].Replace(" ","%s")));

            /*Thread.Sleep(1000);
            proc.StandardInput.WriteLine("shell input keyevent 22");
            Thread.Sleep(1000);
            proc.StandardInput.WriteLine("shell input keyevent 66");
            Thread.Sleep(1000);
            string res = "";
            while (res.IndexOf("input keyevent 66") == -1)
                res = proc.StandardOutput.ReadLine();
            Thread.Sleep(500);
            Console.WriteLine("sms is sent");*/
            proc.Close();
        }
        /*static void Main(string[] args)
        {
            Console.Write("phone number = ");
            string number = Console.ReadLine();
            Console.Write("message = ");
            string message = Console.ReadLine();

            TcpClient tp = new TcpClient();
            try
            {
                tp.Connect("192.168.0.103", 3128);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var stream = tp.GetStream();

            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(String.Format("send_sms;{0};{1}",number,message));
            writer.Flush();
            StreamReader reader = new StreamReader(stream);
            string s = reader.ReadLine();            
        }    */
    }
}
