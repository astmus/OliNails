using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net.Sockets;
using System.IO;


namespace PhoneConnectorHost
{
    class Program
    {
        static void Main(string[] args)
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
        }      
    }   
}
