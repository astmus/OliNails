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
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.FileName = @"c:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe";
            proc.StartInfo.Arguments = "shell";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            while (true)
            {
                proc.StandardInput.WriteLine("am start -a android.intent.action.SENDTO -d sms:+380668851043 --es sms_body asdadasdasd --ez exit_on_sent true");
                proc.StandardInput.Flush();
                Thread.Sleep(500);
                proc.StandardInput.WriteLine("input keyevent 22");
                Thread.Sleep(500);
                proc.StandardInput.WriteLine("input keyevent 66");
                string res = "";
                while (res.IndexOf("input keyevent 66") == -1)
                    res = proc.StandardOutput.ReadLine();
                Thread.Sleep(500);
            }
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
