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
            proc.StartInfo.FileName = @"C:\platform-tools\adb.exe";
            proc.StartInfo.Arguments = "shell";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();

            proc.StandardInput.WriteLine(String.Format("am start -a android.intent.action.SENDTO -d sms:{0} --es sms_body '{1}' --ez exit_on_sent true",args[0],args[1]));
            proc.StandardInput.Flush();
            Thread.Sleep(500);
            proc.StandardInput.WriteLine("input keyevent 22");
            Thread.Sleep(500);
            proc.StandardInput.WriteLine("input keyevent 66");
            string res = "";
            while (res.IndexOf("input keyevent 66") == -1)
                res = proc.StandardOutput.ReadLine();
            Thread.Sleep(500);
            Console.WriteLine("sms is sent");
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
